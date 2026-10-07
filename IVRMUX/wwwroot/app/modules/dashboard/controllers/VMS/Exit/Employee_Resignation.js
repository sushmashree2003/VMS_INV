(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_ResignationController', Employee_ResignationController)
    Employee_ResignationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q','$sce']
    function Employee_ResignationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q, $sce) {

        $scope.submitted = false;
        $scope.validationfordocument = false;
        $scope.obj = {};
        $scope.resignview = false;
        $scope.currentdate = new Date();
        $scope.reliving_date = new Date($scope.currentdate.getFullYear(), $scope.currentdate.getMonth(), $scope.currentdate.getDate() + 90);
        $scope.tentative_Leaving_Date = new Date($scope.reliving_date);
        $scope.savebuttenvalidation = [];
        $scope.CalDate = function (date1, date2) {
            var diffnew = new Date(
                date1.getDay() - date2.getDay()
            );
            var oneDay = 24 * 60 * 60 * 1000;
            var message = Math.round(Math.abs((date1.getTime() - date2.getTime()) / (oneDay)));
            return message;
        };

        $scope.Generate = function () {
            $scope.resignview = true;

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interactedrepeate = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted || field.$dirty;
        };
        
        $scope.loadData = function () {
      
            $scope.savebuttenvalidation1 = [];
            var getid = 2;
            apiService.getURI("Exit_Employee/loadEmployeeData", getid).then(function (promise) {

                if (promise.companydetails != null && promise.companydetails.length > 0) {
                    $scope.companydetails = promise.companydetails[0];
                    $scope.MI_Logo=promise.companydetails[0].mI_Logo;
                }
                $scope.ismcertreQ_AppRejFlag = "";
                if (promise.statusFlag != null && promise.statusFlag.length > 0) {
                    $scope.ismcertreQ_AppRejFlag = promise.statusFlag;
                }
               
                if (promise.reason_list_dd != null && promise.reason_list_dd.length > 0) {
                    $scope.reason_list_dd = promise.reason_list_dd;
                }
                if (promise.resignation_list_dd != null && promise.resignation_list_dd.length > 0) {
                    $scope.resignation_list_dd = promise.resignation_list_dd;
                }

                if (promise.employeedata !== null && promise.employeedata.length > 0) {
                    $scope.ResignationDate
                    $scope.employeename = promise.employeedata[0].employeename;
                    $scope.hrmE_EmployeeFirstName = promise.employeedata[0].employeename;
                    $scope.hrmE_DOJ = promise.employeedata[0].hrmE_DOJ;
                    $scope.hrmdeS_DesignationName = promise.employeedata[0].hrmdeS_DesignationName;
                    $scope.hrmE_EmailId = promise.employeedata[0].hrmE_EmailId;
                    $scope.hrmE_MobileNo = promise.employeedata[0].hrmE_MobileNo;

                    $scope.hrmE_PerStreet = promise.employeedata[0].hrmE_PerStreet;
                    $scope.hrmE_PerArea = promise.employeedata[0].hrmE_PerArea;
                    $scope.hrmE_PerCity = promise.employeedata[0].hrmE_PerCity;
                    $scope.hrmE_PerPincode = promise.employeedata[0].hrmE_PerPincode;
                    $scope.hrmE_Id = promise.employeedata[0].hrmE_Id;
                    $scope.HRME_DOL = promise.employeedata[0].hrmE_DOL;
                    $scope.DateOfJoining = promise.employeedata[0].hrmE_DOJ;
                    $scope.Department = promise.employeedata[0].hrmdeS_DesignationName;
                    $scope.Designation = promise.employeedata[0].hrmdeS_DesignationName;
                }
                if (promise.doc_list != null && promise.doc_list.length > 0) {
                    $scope.documentList = promise.doc_list;

                    angular.forEach($scope.documentList, function (itm) {
                        if (itm.ismresgmcL_OptionalFlg == true) {
                            itm.selectedd2 = true;
                        }
                    });
                }
                if (promise.releavingdoc_list != null && promise.releavingdoc_list.length > 0) {
                    $scope.releavingdoc_list = promise.releavingdoc_list;

                    angular.forEach($scope.releavingdoc_list, function (itm) {
                        if (itm.ismresgmcL_OptionalFlg == true) {
                            itm.selectedd2 = true;
                            $scope.obj.alltaskrel = true;
                        }
                    });
                }

                $scope.showmsgvalidation = false;
                $scope.showmsgvalidationdelete = false;
                if (promise.getGriddata != null && promise.getGriddata.length > 0) {
                    $scope.getGriddata = promise.getGriddata;


                    angular.forEach($scope.getGriddata, function (emp) {
                        if (emp.ismresG_MgmtApprRejFlg === 'Pending' || emp.ismresG_MgmtApprRejFlg === 'Approved' || emp.ismresG_MgmtApprRejFlg === 'Partially Approved') {
                            $scope.savebuttenvalidation.push({ ISMRESG_Id: emp.ismresG_Id, ISMRESG_ResignationDate: emp.ismresG_ResignationDate, ISMRESG_TentativeLeavingDate: emp.ismresG_TentativeLeavingDate });
                        }
                        if (emp.ismresG_MgmtApprRejFlg === 'Approved' || emp.ismresG_MgmtApprRejFlg === '') {
                            $scope.showmsgvalidation = true;
                            $scope.lastworkingday = $scope.savebuttenvalidation[0].ISMRESG_TentativeLeavingDate;
                        }
                        else {
                            $scope.showmsgvalidation = false;
                        }
                        if (emp.ismresG_MgmtApprRejFlg === 'Partially Approved' || emp.ismresG_MgmtApprRejFlg === 'Approved') {
                            $scope.showmsgvalidationdelete = true;
                        }
                        else {
                            $scope.showmsgvalidationdelete = false;
                        }
                    });

                }

                $scope.Showapproval = false;
                $scope.showmsgvalidation1 = false;
                if (promise.getGriddataReleaving != null && promise.getGriddataReleaving.length > 0) {
                    $scope.getGriddataReleaving = promise.getGriddataReleaving;
                    angular.forEach($scope.getGriddataReleaving, function (emp) {
                        if (emp.ismresG_MgmtApprRejFlg === 'Pending' || emp.ismresG_MgmtApprRejFlg === 'Approved' || emp.ismresG_MgmtApprRejFlg === 'Partially Approved') {
                            $scope.savebuttenvalidation1.push({ document_Path: emp.ISMRESGMCL_Template, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                        if (emp.ismresG_MgmtApprRejFlg === 'Approved') {
                            $scope.showmsgvalidation1 = true;
                        }
                        else {
                            $scope.showmsgvalidation1 = false;
                        }
                        if (emp.ismresG_MgmtApprRejFlg === 'Partially Approved' || emp.ismresG_MgmtApprRejFlg === 'Approved') {
                            $scope.showmsgvalidationdeletereling = true;
                        }
                        else {
                            $scope.showmsgvalidationdeletereling = false;
                        }
                        if ( emp.ismresG_MgmtApprRejFlg != 'Approved') {
                            $scope.Showapproval = true;
                        }
                    });
                }
                if (promise.managmentempdetails != null && promise.managmentempdetails.length > 0) {
                    $scope.managmentempdetails = promise.managmentempdetails;
                    $scope.employeeTL = promise.managmentempdetails[0].Deptheadname;
                    $scope.teamleadEmailId = promise.managmentempdetails[0].Deptheadmailid;
                    $scope.employeeManager = promise.managmentempdetails[0].Hrname;
                    $scope.mngrEmailId = promise.managmentempdetails[0].Hrmailid;
                }
                var joindate = new Date($filter('date')(new Date(promise.employeedata[0].hrmE_DOJ).toDateString(), "yyyy/MM/dd"));
                var leftdate = new Date($filter('date')(new Date($scope.currentdate).toDateString(), "yyyy/MM/dd"));
                var exp = $scope.CalDate(leftdate, joindate);
                $scope.duration = exp + 1;
                if (promise.getdeviationreport.length > 0) {
                    $scope.getdeviationreport = promise.getdeviationreport[0];
                    $scope.devaionvalidation = true;
                }
                else {
                    $scope.devaionvalidation = false;
                }

               
            });
        };

        $scope.deletechecklistpopup = function () {
            swal("You Can't Delete the Checklist. Because Your Application is Approved");
        }
        $scope.noticeperiodcalculation = function () {
            angular.forEach($scope.resignation_list_dd, function (itm) {
                if ($scope.ismresgmrE_Id == itm.ismresgmrE_Id) {
                    $scope.noticeperiod = itm.ismresgmrE_NoticePeriodDays
                }

            });

            //if ($scope.devaionvalidation == true) {
            //    swal("Sorry!! Since Your Deviation is more than 0%, You are not eligible to Apply Resignation.");
            //    $state.reload();
            //}
        }
        //Resignation checklist
        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            var checkStatus2 = $scope.obj.alltask;
            angular.forEach($scope.documentList, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {
            $scope.usercheckC2 = $scope.documentList.every(function (options) {
                return options.selectedd2;
            });
        };

        //Releaving checklist


        $scope.selectedAll1 = "";
        $scope.all_taskrel = function () {
            var checkStatus2 = $scope.obj.alltaskrel;
            angular.forEach($scope.releavingdoc_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.selectedAll1 = "";
        $scope.releavingall_task = function () {
            var checkStatus2 = $scope.obj.releavingall_task;
            angular.forEach($scope.releavingdoc_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.usercheckC2 = "";
        $scope.releavingget_evalistt = function () {
            $scope.usercheckC2 = $scope.releavingdoc_list.every(function (options) {
                return options.selectedd22;
            });
        };

        //Resignation Applye
        $scope.SendMail = function (Restype) {
         //   $scope.validationfordocument = true;
            $scope.tmplt = [];
            $scope.doc_list2 = [];
            var Template = document.getElementById("calltemplate").innerHTML;
            $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

            if ($scope.myForm.$valid) {
                angular.forEach($scope.documentList, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        if (emp.document_Path.length > 0) {
                            $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                        else {
                            $scope.doc_list2.push({ document_Path: emp.ISMRESGMCL_Template, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                    }
                });

                var data = {
                    "ManagerMailid": $scope.mngrEmailId,
                    "TeamLeadMailid": $scope.teamleadEmailId,
                    "ISMRESGMRE_Id": $scope.ismresgmrE_Id,
                    "ISMRESG_Remarks": $scope.ISMRESG_Remarks,
                    "ISMRESG_NoticePeriod": $scope.noticeperiod,
                    "ISMRESG_Flg": Restype,
                    doc_list2: $scope.doc_list2,
                    Template: Template
                };
                apiService.create("Exit_Employee/sendResignationMail", data).
                    then(function (promise) {
                        if (promise.retrunMsg == "Success") {
                            swal('Resignation Sent Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnval == "Exist") {
                            swal('Resignation Already Applied.');
                        }
                        else if (promise.returnval == "AddedChecklist") {
                            swal('Checklist Uploaded Successfully..');
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed!!');
                        }
                    });
            }
            else { $scope.submitted = true; }
        };

        $scope.cancel = function () {
            $state.reload();
            $scope.validationfordocument = false;
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("calltemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        // ===============================================Upload=====================
        $scope.selectFileforUploadzdOtherDetail = [];

        $scope.selectFileforUploadzd1 = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    data.document_Path1 = d;
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.document_Path1 = d;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //===========================Upload======================================
        $scope.SelectedFileForUploadzdd = [];
        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzdd = input.files;

            if (input.files && input.files[0]) {
                

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                Uploadprofiled(document);
            }
        };



        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.SelectedFileForUploadzdd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.ismresgmcL_Id);
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
                    $scope.document_Path1 = d;
                    var nameArray = $scope.document_Path1.split('.');
                    $scope.extention = nameArray[nameArray.length - 1];
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        $scope.showmodaldetails1 = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');

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
            else if (extention == "xlsx" || extention == "xls" || extention == "ppt" || extention == "zip" || extention == "doc" || extention == "docx" || extention == "txt") {
                //  $window.open(imagepreview)
                data = "https://view.officeapps.live.com/op/view.aspx?src=" + data;
                $scope.detailFrame = $sce.trustAsResourceUrl(data);
                $('#myModaldocx').modal('show');
            }
        };



        $scope.showmodaldetailscheck = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
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
            else if (extention == "xlsx" || extention == "xls" || extention == "ppt" || extention == "zip" || extention == "doc" || extention == "docx" || extention == "txt") {
              //  $window.open(imagepreview)
                data = "https://view.officeapps.live.com/op/view.aspx?src=" + data;
                $scope.detailFrame = $sce.trustAsResourceUrl(data);
                $('#myModaldocx').modal('show');
            }
        };




        //View UPLOADED Documents
        $scope.viewcomment = function (user) {
            $scope.uploadeddocumets = [];
            var data = {
                "ISMRESG_Id": user.ismresG_Id,
            };
            apiService.create("Exit_Employee/chcklistdocs", data).then(function (promise) {
                if (promise.uploadeddocumets.length > 0 || promise.uploadeddocumets == null) {
                    $scope.uploadeddocumets = promise.uploadeddocumets;
                }

            });
        };

        //Delete uploaded Checklist Documents
        $scope.deletechecklist = function (user) {
            var data = {
                "ISMRESGCL_Id": user.ismresgcL_Id,
            };
            apiService.create("Exit_Employee/chcklistdocs", data).then(function (promise) {
                if (promise.returnval == "Update") {
                    swal('Checklist Deleted Successfully.');
                    $state.reload();
                }
                else if (promise.returnval == "notUpdated") {
                    swal('Checklist Not Deleted Successfully.');
                }
                else {
                    swal('Operation Failed!!');
                }

            });
        };

        //View APPROVED PERSON
        $scope.Approvedperson = function (user) {
            $scope.apporvedrejectedperson = [];
            var data = {
                "ISMRESG_Id": user.ismresG_Id,
            };
            apiService.create("Exit_Employee/chcklistdocs", data).then(function (promise) {
                if (promise.apporvedrejectedperson.length > 0 || promise.apporvedrejectedperson == null) {
                    $scope.apporvedrejectedperson = promise.apporvedrejectedperson;
                }

            });
        };

        $scope.savechecklists = function () {
            $scope.doc_list2 = [];
            angular.forEach($scope.documentList, function (emp) {
                if (emp.selectedd2 === true || emp.document_Path != null) {
                    $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                }
            });
            var data = {
                doc_list2: $scope.doc_list2,
            };
            apiService.create("Exit_Employee/savechecklists", data).then(function (promise) {
                if (promise.returnval == "Add") {
                    swal('Checklist Uploaded Successfully.');
                    $state.reload();
                }
                else if (promise.returnval == "notUpdated") {
                    swal('Checklist Not Uploaded Successfully.');
                }
                else {
                    swal('Operation Failed!!');
                }
            });
        };

        $scope.Releavingapplye = function (Restype) {
            $scope.doc_list2 = [];
            if ($scope.myForm2.$valid) {
                angular.forEach($scope.releavingdoc_list, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                    }
                });
                var data = {
                   // "ISMRESGMRE_Id": $scope.obj.releavingismresgmrE_Id,
                    "ISMRESG_Flg": Restype,
                    "ISMRESG_Remarks": $scope.obj.ismresG_Remarks,
                    doc_list2: $scope.doc_list2,
                };
                apiService.create("Exit_Employee/Exit_empl_SaveEdit", data).then(function (promise) {
                    if (promise.returnval == "AddedChecklist") {
                        swal('Checklist Uploaded Successfully.');
                        $state.reload();
                    }
                    else if (promise.returnval == "notUpdated") {
                        swal('Checklist Not Uploaded Successfully.');
                    }
                    else if (promise.returnval == "Add") {
                        swal('Application  Saved Successfully.');
                        $state.reload();
                    }
                    else if (promise.returnval == "notUpdated") {
                        swal('Application Not Saved Successfully.');
                    }
                    else {
                        swal('Operation Failed!!');
                    }
                });
            }
            else { $scope.submitted = true; }
        };



        //Checklist Upload
        $scope.uploadchecklist = function (Restype) {
            $scope.doc_list2 = [];
            if (Restype == "Resignation") {
                angular.forEach($scope.documentList, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        if (emp.document_Path.length > 0) {
                            $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                        //else {
                        //    $scope.doc_list2.push({ document_Path: emp.ISMRESGMCL_Template, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        //}
                    }
                });
            }
            else {
                angular.forEach($scope.releavingdoc_list, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        if (emp.document_Path.length > 0) {
                            $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                        //else {
                        //    $scope.doc_list2.push({ document_Path: emp.ISMRESGMCL_Template, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        //}
                    }
                });
            }


            if ($scope.doc_list2.length > 0) {
                var data = {
                    "ISMRESGMRE_Id": $scope.obj.releavingismresgmrE_Id,
                    "ISMRESG_Flg": Restype,
                    doc_list2: $scope.doc_list2,
                };
                apiService.create("Exit_Employee/Exit_empl_SaveEdit", data).then(function (promise) {
                    if (promise.returnval == "AddedChecklist") {
                        swal('Checklist Uploaded Successfully.');
                        $state.reload();
                    }
                    else if (promise.returnval == "notUpdated") {
                        swal('Checklist Not Uploaded Successfully.');
                    }
                    else {
                        swal('Operation Failed!!');
                    }
                });
            }
            else {
                swal('Select Atleast One Checklist For Upload!!');
            }

        }





        $scope.nocdetails = function () {
            var data = {
                "HRME_Id": $scope.hrmE_Id,
            };
            apiService.create("HR_NOC_Process/nocdetails", data).then(function (promise) {

                if (promise.aprovedlist != null && promise.aprovedlist != undefined && promise.aprovedlist.length > 0) {
                    $scope.nocapproveddetails = promise.aprovedlist;
                }
                if (promise.checklist != null && promise.checklist != undefined && promise.checklist.length > 0) {
                    $scope.checklistapprovedlist = promise.checklist;
                }
            });
        };



    }
})();