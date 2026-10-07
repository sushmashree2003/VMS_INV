(function () {
    'use strict';
    angular.module('app').controller('ISM_Welcome_Closing_LetterController', ISM_Welcome_Closing_LetterController)
    ISM_Welcome_Closing_LetterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$sce', '$compile']
    function ISM_Welcome_Closing_LetterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $sce, $compile) {

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            paginationformasters = 10;
        } else {
            paginationformasters = 10;
        }


        $scope.currentdate = new Date().toLocaleDateString();
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.getPaginatedData = function () {
            var startIndex = ($scope.currentPage - 1) * $scope.itemsPerPage;
            return $scope.versionreleasedata.slice(startIndex, startIndex + $scope.itemsPerPage);
        };

        $scope.changePage = function (pageNumber) {
            $scope.currentPage = pageNumber;
        };

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.submitted1 = false;
        $scope.temptaskreponsefiles = [];
        $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
        $scope.objques = {};
        $scope.VisitedDate = new Date();
        $scope.visitedmaxdate = new Date();
        $scope.searchchkbx = "";
        $scope.ISMSLE_Id = 0;
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.getloaddata, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.getloaddata.every(function (options) {
                return options.selected;
            });
        }

        $scope.search = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.ismcltprP_PaymentDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (JSON.stringify(obj.ismcltprP_InstallmentAmt)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ISMMCLT_ClientName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismmpR_ProjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismcltprP_InstallmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.LoadHWHealthCheckUpdata = function () {
            $scope.getuserinstitution = [];
            $scope.MI_Id = "";
            $scope.Remarks = "";
            $scope.search4 = "";
            $scope.getclientlist = [];
            $scope.getloaddata = [];
            $scope.objques.ISMMCLT_Id = "";
            $scope.objques.HRME_Id = "";
            $scope.getemployeedetails = [];
            $scope.submitted1 = false;
            $scope.editable = false;
            $scope.HWHCHUP_Id = 0;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            var data = {
                "OnLoadOrOnChange": "OnLoad"
            };

            apiService.create("HW_HeathCheckUp/OnLoadHWHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.getclientlist = promise.getclientlist;
                    $scope.getloaddata = promise.detailsarray;
                    $scope.yearlist = promise.viewcommens;
                    $scope.getemployeedetails = promise.getemployeedetails;
                    $scope.welcomeclose = promise.welcomeclosearray;

                }
            });
        };
        $scope.employee = function (selectedEmployee) {

            $scope.datanew = selectedEmployee.hrmE_EmployeeFirstName;
        }

        //added by praveenkumar 
        $scope.bindtemplate12345 = []; $scope.xtra = "";
        $scope.loadtemplate = function () {
            $scope.ISMSLE_Id = 0;
            $scope.totalAmount = 0;
            $scope.TotalBasicAmount = 0;
            $scope.TotalTaxAmount = 0;
            $scope.ReceivedAmount = 0;
            $scope.TDSAmount = 0;
            $scope.Balance = 0;
            var items = {
                "Ayear": $scope.ASMAY_ID,
                "pending": $scope.ISMPCL_TemplateName,
                "msg": "FetchTemp",
                "ISMMCLT_Id": $scope.objques.ISMMCLT_Id.ismmclT_Id

            };
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate12345"));
                if (promise.get_projectdetail != null && promise.get_projectdetail.length > 0) {
                    $scope.bindtemplate12345 = promise.get_projectdetail;
                    $scope.xtra = $scope.bindtemplate12345[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                    $scope.closing_letter = promise.closing_letter
                    $scope.Sms_Quantity = promise.sms_Quantity
                    $scope.Offt_letter = promise.offt_letter
                    $scope.Onnt_letter = promise.onnt_letter
                    $scope.ICC_letter = promise.icc_letter
                    $scope.Moudetails = promise.mou_details
                    $scope.Patch_release = promise.patch_release
                    $scope.Custamization = promise.custamization
                    $scope.It_HealthCheckup = promise.it_HealthCheckup
                    $scope.Month_yearreprot = promise.month_yearreprot
                    $scope.Man_power = promise.man_power
                    $scope.Feedback_System = promise.feedback_System
                    $scope.service_System = promise.service_System
                    $scope.components_docs = promise.components_docs
                    $scope.Replacement_data = promise.client_list   // existing array
                    $scope.Attendancelist = promise.attendancelist
                    $scope.Invoicedata = promise.invoicedata
                    $scope.Telecallerlist = promise.telecallerlist
                    $scope.WelcomePatchrelease = promise.welcomePatchrelease
                    $scope.WelcomeHealthCheckup = promise.welcomeHealthCheckup
                    $scope.WelcomeManPower = promise.welcomeManPower
                    $scope.WelcomeAmount = promise.welcomeAmount
                    $scope.WelcomeImplementation = promise.welcomeImplementation
                    $scope.Welcomebasicdata = promise.welcomebasicdata;
                    $scope.Custamization_report = promise.custamization_report
                    $scope.Payment = promise.payment;
                    $scope.WelcomeSms = promise.welcomeSms;
                    $scope.WelcomeOfflinetraining = promise.welcomeOfflinetraining;
                    $scope.WelcomeOnlinetraining = promise.welcomeOnlinetraining;
                    $scope.WelcomeCustom = promise.welcomeCustom;
                    $scope.WelcomeIENAME = promise.welcomeIENAME;

                    angular.forEach($scope.Payment, function (cls) {
                        $scope.totalAmount += cls.ISMINC_TotalAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TotalBasicAmount += cls.ISMINC_TotalBasicAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TotalTaxAmount += cls.ISMPRINC_TotalTaxAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.ReceivedAmount += cls.ISMCPPD_ReceivedAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TDSAmount += cls.ISMCPPD_TDSAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.Balance += cls.Balance;
                    });
                    //Welcome data
                    $("#template12345").modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }


        $scope.SendDataview = function () {

            $scope.ISMMCLTPR_NodalOfficerEmailId = $scope.Moudetails[0].ISMMCLT_EmailId;
            $scope.ISMMCLTPR_NodalOfficerContactNo = $scope.Moudetails[0].ISMMCLT_ContactNo;

            $scope.taxdto = [];
            var msg = document.getElementById("MoutempEmail").innerHTML;
            var data = {
                "msg": msg,
                "ISMMCLTPR_NodalOfficerEmailId": $scope.ISMMCLTPR_NodalOfficerEmailId,
                "ISMMCLTPR_NodalOfficerContactNo": $scope.ISMMCLTPR_NodalOfficerContactNo,
            //    "ISMMCLTPR_NodalOfficerEmailId": " hutchingshighschoolpune@gmail.com",
             //   "ISMMCLTPR_NodalOfficerContactNo": 9890827823,

            };
            apiService.create("ISM_ClientProject_Mapping/sendMail", data).then(function (promise) {
                swal("Email Sent SucessFully ! ");
                $('#OrderAcceptance').modal('hide');
                $scope.clearFunction();

            });

        }



        //VIEW OPTION AFTER SAVING CLOSING AND WELCOME LETTER 

        $scope.bindtemplate12345 = []; $scope.xtra = "";
        $scope.Viewtemplateone2 = function (itm) {

            var items = {
                "Ayear": itm.ASMAY_ID,
                "pending": itm.ISMPCL_TemplateName,
                "msg": "FetchTemp",
                "ISMMCLT_Id": itm.ISMMCLT_Id

            };
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate12345"));
                if (promise.get_projectdetail != null && promise.get_projectdetail.length > 0) {
                    $scope.bindtemplate12345 = promise.get_projectdetail;
                    $scope.xtra = $scope.bindtemplate12345[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                    $scope.closing_letter = promise.closing_letter
                    $scope.Sms_Quantity = promise.sms_Quantity
                    $scope.Offt_letter = promise.offt_letter
                    $scope.Onnt_letter = promise.onnt_letter
                    $scope.ICC_letter = promise.icc_letter
                    $scope.Moudetails = promise.mou_details
                    $scope.Patch_release = promise.patch_release
                    $scope.Custamization = promise.custamization
                    $scope.It_HealthCheckup = promise.it_HealthCheckup
                    $scope.Month_yearreprot = promise.month_yearreprot
                    $scope.Man_power = promise.man_power
                    $scope.Feedback_System = promise.feedback_System
                    $scope.service_System = promise.service_System
                    $scope.components_docs = promise.components_docs
                    $scope.Replacement_data = promise.client_list   // existing array
                    $scope.Attendancelist = promise.attendancelist
                    $scope.Invoicedata = promise.invoicedata
                    $scope.Telecallerlist = promise.telecallerlist

                    $scope.WelcomePatchrelease = promise.welcomePatchrelease
                    $scope.WelcomeHealthCheckup = promise.welcomeHealthCheckup
                    $scope.WelcomeManPower = promise.welcomeManPower
                    $scope.WelcomeAmount = promise.welcomeAmount
                    $scope.WelcomeImplementation = promise.welcomeImplementation
                    $scope.Welcomebasicdata = promise.welcomebasicdata;
                    $scope.Custamization_report = promise.custamization_report
                    $scope.Payment = promise.payment;
                    $scope.WelcomeSms = promise.welcomeSms;
                    $scope.WelcomeOfflinetraining = promise.welcomeOfflinetraining;
                    $scope.WelcomeOnlinetraining = promise.welcomeOnlinetraining;
                    $scope.WelcomeCustom = promise.welcomeCustom;
                    $scope.WelcomeIENAME = promise.welcomeIENAME;


                    angular.forEach($scope.Payment, function (cls) {
                        $scope.totalAmount += cls.ISMINC_TotalAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TotalBasicAmount += cls.ISMINC_TotalBasicAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TotalTaxAmount += cls.ISMPRINC_TotalTaxAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.ReceivedAmount += cls.ISMCPPD_ReceivedAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.TDSAmount += cls.ISMCPPD_TDSAmount;
                    });
                    angular.forEach($scope.Payment, function (cls) {
                        $scope.Balance += cls.Balance;
                    });
                    //Welcome data

                    $("#template12345").modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }

        $scope.OnChangeHwHealthCheckUpInst = function () {
            $scope.getclientlist = [];
            $scope.objques.ISMMCLT_Id = "";
            $scope.search4 = "";
            $scope.Remarks = "";
            $scope.editable = false;
            $scope.submitted1 = false;
            $scope.HWHCHUP_Id = 0;
            $scope.temptaskreponsefiles = [];
            $scope.getloaddata = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            $scope.VisitedDate = new Date();
            $scope.visitedmaxdate = new Date();
            $scope.objques.HRME_Id = "";
            $scope.getemployeedetails = [];
            var data = {
                "OnLoadOrOnChange": "OnChange",
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/OnLoadHWHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getclientlist = promise.getclientlist;
                    $scope.getloaddata = promise.getloaddata;
                    $scope.getemployeedetails = promise.getemployeedetails;
                }
            });
        };

        $scope.welclosesavedata = function () {
            $scope.submitted1 = false;
            if ($scope.myForm1.$valid) {
                var data = {

                    "ISMMCLT_Id": $scope.objques.ISMMCLT_Id.ismmclT_Id,
                    "ISMPCL_TemplateName": $scope.ISMPCL_TemplateName,
                    "ASMAY_ID": $scope.ASMAY_ID,

                };
                apiService.create("HW_HeathCheckUp/welclosesavedata", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.msg === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Record Saved/Updated Successfully");
                        }


                        $scope.OnChangeHwHealthCheckUpInst();
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.showmothersign1 = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };

        $scope.closeimg13 = function () {

            $scope.ISMPCL_TemplateName = "";
        };
        $scope.onviewpdf1 = function (filepath) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfIdzz").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfIdzz").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                    $('#modalviewfiles').modal('hide');

                });
        };

        $scope.closeimgnew = function () {
            $('#showpdf').modal('hide');
            ////$scope.ISMPCL_TemplateName = "";
            $('#template12345').modal('show');
        }
        $scope.closeimg = function () {
            $('#template12345').modal('show');
            $('#myModalPreview').modal('hide');
            $scope.ISMPCL_TemplateName = "";

        }
        $scope.printToCart = function () {
            var innerContents = document.getElementById("MoutempEmail").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.EditHealthCheckUp = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/EditHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.HWHCHUP_Id = $scope.geteditdetails[0].hwhchuP_Id;
                    $scope.Remarks = $scope.geteditdetails[0].remarks;
                    $scope.VisitedDate = new Date($scope.geteditdetails[0].visitedDate);

                    $scope.geteditclientdetails = promise.geteditclientdetails;
                    $scope.geteditemployeedetails = promise.geteditemployeedetails;
                    $scope.objques.ISMMCLT_Id = $scope.geteditclientdetails[0];
                    $scope.objques.HRME_Id = $scope.geteditemployeedetails[0];

                    if (promise.geteditfilesdetails !== null && promise.geteditfilesdetails.length > 0) {
                        $scope.temptaskreponsefiles = promise.geteditfilesdetails;
                        angular.forEach($scope.temptaskreponsefiles, function (dd) {
                            if (dd.filepath !== null && dd.filepath !== "") {
                                var img = dd.filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };

        //Added by ganesh


        $scope.releaseversion = function (patch) {
            $scope.submitted1 = false;
            if ($scope.myForm1.$valid) {
                var data = {

                    "IVRMVT_ID": patch.IVRMVT_ID,
                    "ISMMCLT_Id": patch.ISMMCLT_Id,

                };
                apiService.create("ISM_ClientProject_Mapping/releaseversion", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.versionreleasedata = promise.versionreleasedata;
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };


        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "ISMPSSUP_Id": obj.ISMPSSUP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewdocumetsignoff !== null && promise.getviewdocumetsignoff.length > 0) {
                        $scope.getviewdocumetsignoff = promise.getviewdocumetsignoff;
                        angular.forEach($scope.getviewdocumetsignoff, function (dd) {
                            if (dd.filepath !== null && dd.filepath !== "") {
                                var img = dd.filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
                }
            });
        };

        $scope.ActiveDeactiveHealthCheckUp = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.activeFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HW_HeathCheckUp/ActiveDeactiveHealthCheckUp", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.OnChangeHwHealthCheckUpInst();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeleteHWHealthCheckUpFiles = function (obj) {

            var mgs = "";
            var confirmmgs = "";

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "HWHCHUPF_Id": obj.hwhchupF_Id,
                "MI_Id": $scope.MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HW_HeathCheckUp/DeleteHWHealthCheckUpFiles", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Failed To Delete Record");
                            }
                            if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                                $scope.getviewfilesdetails = promise.getviewfilesdetails;
                                angular.forEach($scope.getviewfilesdetails, function (dd) {
                                    if (dd.filepath !== null && dd.filepath !== "") {
                                        var img = dd.filepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        console.log("dd.filetype : " + dd.filetype);
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                        }
                                    }
                                });
                                $('#modalviewfiles').modal('show');
                            } else {
                                $scope.OnChangeHwHealthCheckUpInst();
                                //swal("No Records Found");
                                $('#modalviewfiles').modal('hide');
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.ViewHealthCheckUpComments = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpComments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewcommens !== null && promise.viewcommens.length > 0) {
                        $scope.viewcommens = promise.viewcommens;
                        $('#modalviewcomments').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewcomments').modal('hide');
                    }
                }
            });
        };

        $scope.ViewFile = function (docpath, filename, filetype) {

            if (filetype === 'Excel') {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
                $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
                $('#myModaldocx').modal('show');
            } else {
                var imagedownload = docpath;
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
        };

        $scope.DownloadFile = function (data, idd, filetype) {
            var file_type = "";
            if (filetype === "Pdf") {
                file_type = '.pdf';
            } else {
                file_type = '.xls';
            }
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg + file_type
                })[0].click();
            });
        };

        //$scope.showmothersign = function (path) {
        //    $('#preview').attr('src', path);
        //    $('img').bind('contextmenu', function (e) {
        //        return false;
        //    });
        //    $('#myModalPreview').modal('show');
        //};

        //$scope.onviewpdf = function (filepath, filename) {
        //    var imagedownload = filepath;
        //    $scope.content = "";
        //    var fileURL = "";
        //    var file = "";
        //    document.getElementById("pdfviewdd").innerHTML = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.content = $sce.trustAsResourceUrl(fileURL);
        //            var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
        //            document.getElementById("pdfviewdd").innerHTML = htmlElements;
        //            $('#showpdf').modal('show');
        //        });
        //};

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.Clearid = function () {
            $scope.OnChangeHwHealthCheckUpInst();
        };

        $scope.addNewviewresponse = function () {
            var newItemNo = $scope.temptaskreponsefiles.length + 1;

            if (newItemNo <= 10) {
                $scope.temptaskreponsefiles.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.temptaskreponsefiles);
        };

        $scope.removeNewviewresponse = function (index) {
            var newItemNo = $scope.temptaskreponsefiles.length - 1;
            $scope.temptaskreponsefiles.splice(index, 1);

            if ($scope.temptaskreponsefiles.length === 0) {
                //data
            }
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
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
            $http.post("/api/ImageUpload/HWHealthCheckUpFiles", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.filepath = d;
                    data.filename = $scope.filename;
                    $('#').attr('src', data.filepath);
                    var img = data.filepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.filepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //$scope.modelclose = function () {
        //    $('#myModal3').modal('hide');
        //    $('#template12345').modal('show');  
        //}

        $scope.modelclose = function () {

            $('#template12345').modal('show');
            $('#myModal3').modal('hide');
            $scope.ISMPCL_TemplateName = "";

        }
    }



})();