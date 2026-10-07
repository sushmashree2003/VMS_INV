(function () {
    'use strict';
    angular.module('app').controller('FMS_CorrespondenceController', FMS_CorrespondenceController)
    FMS_CorrespondenceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$sce', '$timeout']
    function FMS_CorrespondenceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $sce, $timeout) {

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
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

        $scope.getserverdate();

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.obj = {};
        $scope.objfiles = {};
        $scope.objmode = {};
        $scope.editable = false;
        $scope.editableflag = false;
        $scope.editableflagmode = false;
        $scope.savebtn = false;
        $scope.obj.FMSCOR_Date = new Date($scope.today);
        $scope.maxdFMSCOR_Date = new Date($scope.today);
        $scope.labeldetails = "Client";

        $scope.obj.searchrefno = "";
        $scope.objfiles.searchrefnotab2 = "";
        $scope.objmode.searchrefnotab3 = "";        

        /* FIRST TAB */
        $scope.onfirsttabcorrespondence = function () {
            var pageid = 2;
            $scope.labeldetails = "Client";
            $scope.obj.FMSCOR_ClientSupplierFlg = 'Client';
            $scope.submitted = false;
            $scope.client_Supplier_SalesLead_code = "";
            $scope.HRMD_Id = "";
            $scope.clear_first_tab_load();
            apiService.getURI("FMS_Correspondence/onfirsttabcorrespondence", pageid).then(function (promise) {
                if (promise.messageval === "NoPrivileges") {
                    swal("Department Is Not Mapped For This User");
                } else if (promise.messageval === "NotMapped") {
                    swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                } else if (promise.messageval === "Mapped") {
                    $scope.getfilecategory = promise.getfilecategory;
                    $scope.getclient = promise.getclient;
                    $scope.getsaleslead = promise.getsaleslead;
                    $scope.getsuppliers = promise.getsuppliers;
                    $scope.getfirstgriddata = promise.getfirstgriddata;
                    $scope.getemployeelist = promise.getemployeelist;
                    $scope.deptlist = promise.institutiondeptdetails;
                    $scope.getproject = promise.getproject;
                    $scope.HRMD_Id = promise.hrmD_Id;

                    angular.forEach($scope.getemployeelist, function (d) {
                        d.FMSCOREMP_Id = 0;
                        d.empck = false;
                    });

                    if (promise.getgeneralconfig !== null && promise.getgeneralconfig.length > 0) {
                        $scope.FMSManualReferenceFlg = promise.getgeneralconfig[0].ivrmgC_FMSManualReferenceFlg;
                        $scope.savebtn = false;
                        if ($scope.FMSManualReferenceFlg === true) {
                            $scope.manualorautorefno = "Auto";
                            $scope.autorefno = true;
                        }
                        else if ($scope.FMSManualReferenceFlg === false) {
                            $scope.manualorautorefno = "Manual";
                            $scope.autorefno = false;
                        } else {
                            $scope.savebtn = true;
                            swal("Map The Auto/Manual Reference No. In General Settings");
                        }
                    } else {
                        swal("General Setting Not Done For This Institution");
                    }
                } else {
                    swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                }
            });
        };

        $scope.onchnageradiobtns = function () {
            $scope.obj.ismmclT_Id = "";
            $scope.obj.INVMS_Id = "";
            $scope.obj.ISMSLE_Id = "";
            $scope.obj.ISMMPR_Id = "";
            $scope.obj.FMSCOR_RefernceNo = "";
            $scope.labeldetails = "";
            $scope.obj.FMSCOR_LetterHeadNo = "";
            $scope.obj.FMSCOR_Description = "";
            $scope.labeldetails = $scope.obj.FMSCOR_ClientSupplierFlg;
            $scope.client_Supplier_SalesLead_code = "";
            $scope.savebtn = false;

        };

        $scope.onchangefilecategory = function () {

            $scope.obj.FMSCOR_Subject = "";
            $scope.obj.FMSCOR_Date = new Date($scope.today);
            $scope.obj.ISMMCLT_Id = "";
            $scope.obj.INVMS_Id = "";
            $scope.obj.ISMSLE_Id = "";
            $scope.obj.ISMMPR_Id = "";
            $scope.obj.FMSCOR_RefernceNo = "";
            $scope.client_Supplier_SalesLead_code = "";
            $scope.client_Supplier_SalesLead_code = "";
            $scope.savebtn = false;           
        };

        $scope.onchangeclient = function (obj) {
            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });
            $scope.savebtn = false;
            $scope.clientcode = "";
            $scope.client_Supplier_SalesLead_code = "";
            angular.forEach($scope.getclient, function (d) {
                if (d.ismmclT_Id === parseInt(obj.ismmclT_Id)) {
                    $scope.clientcode = d.ismmclT_Code;
                    $scope.client_Supplier_SalesLead_code = d.ismmclT_Code;
                }
            });
            var i = 0;
            if ($scope.clientcode === null || $scope.clientcode === "" || $scope.clientcode === "0") {
                i = 1;
                $scope.savebtn = true;
                swal("Client Code Is Not Entered , So Kindly Enter Client Code For Generating Reference Number");
            }
            if (i === 0) {
                var data = {
                    "ISMMCLT_Id": obj.ismmclT_Id,
                    "MI_Id": $scope.miid
                };
                apiService.create("FMS_Correspondence/onchangeclient", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getproject_temp = promise.getproject;
                        if ($scope.getproject_temp !== null && $scope.getproject_temp.length > 0) {
                            $scope.getproject = promise.getproject;
                        } else {
                            swal("No Project Is Mapping For This Client");
                        }
                    }
                });
            }
        };

        $scope.OnChangeSupplier = function (obj) {
            $scope.SupplierCode = "";
            $scope.client_Supplier_SalesLead_code = "";
            $scope.savebtn = false;
            angular.forEach($scope.getsuppliers, function (d) {
                if (d.invmS_Id === parseInt(obj.invmS_Id)) {
                    $scope.SupplierCode = d.invmS_SupplierCode;
                    $scope.client_Supplier_SalesLead_code = d.invmS_SupplierCode;
                }
            });
            var i = 0;
            if ($scope.SupplierCode === null || $scope.SupplierCode === "" || $scope.SupplierCode === "0") {
                i = 1;
                $scope.savebtn = true;
                swal("Supplier Code Is Not Entered , So Kindly Supplier Code For Generating Reference Number");
            }
        };



        $scope.OnChangeSalesLead = function (id) {
            var data = {
                "MI_Id": id.MI_Id,
                "ISMSLE_Id": id.ISMSLE_Id
            }
            apiService.create("FMS_Correspondence/getleaddetails", data).then(function (promise) {
                if (promise.get_lead_details != null && promise.get_lead_details.length > 0) {
                    $scope.get_lead_details = promise.get_lead_details;
                    $scope.ISMSLE_ContactPerson = $scope.get_lead_details[0].ISMSLE_ContactPerson;

                    angular.forEach($scope.get_lead_details, function (d) {
                        if (d.ISMSLE_Id === parseInt(id.ISMSLE_Id)) {
                            $scope.obj.FMSCOR_ToName = d.ISMSLE_ContactPerson;
                            //$scope.obj.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            //$scope.obj.ISMSLE_LeadName = d.ISMSLE_LeadName;

                            $scope.obj.FMSCOR_Subject = d.ISMSMPR_ProductName;
                        }
                    });



                }
            });
        };



        //$scope.OnChangeSalesLead = function (obj) {
        //    $scope.savebtn = false;
        //    $scope.SalesLeadCode = "";
        //    $scope.client_Supplier_SalesLead_code = "";
        //    angular.forEach($scope.getsaleslead, function (d) {
        //        if (d.ismslE_Id === parseInt(obj.ismslE_Id)) {
        //            $scope.SalesLeadCode = d.ismslE_LeadCode;
        //            $scope.client_Supplier_SalesLead_code = d.ismslE_LeadCode;
        //        }
        //    });
        //    var i = 0;
        //    if ($scope.SalesLeadCode === null || $scope.SalesLeadCode === "" || $scope.SalesLeadCode === "0") {
        //        i = 1;
        //        $scope.savebtn = true;
        //        swal("Sales Lead Code Is Not Entered , So Kindly Sales Lead Code For Generating Reference Number");
        //    }
        //};

    


        $scope.checkrefnoduplicate = function () {
            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            var data = {
                "FMSCOR_RefernceNo": $scope.obj.FMSCOR_RefernceNo,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/checkrefnoduplicate", data).then(function (promise) {
                if (promise !== null) {
                    $scope.messageval = promise.messageval;
                    if ($scope.messageval === "Duplicate") {
                        $scope.obj.FMSCOR_RefernceNo = "";
                        swal("Refernce Number Already Exists");
                    }
                }
            });
        };

        $scope.OnChangeLetterHeadNo = function () {

            if ($scope.obj.FMSCOR_LetterHeadNo !== undefined && $scope.obj.FMSCOR_LetterHeadNo !== null && $scope.obj.FMSCOR_LetterHeadNo !== "") {
                angular.forEach($scope.deptlist, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid = dd.mI_Id;
                    }
                });

                var data = {
                    "FMSCOR_LetterHeadNo": $scope.obj.FMSCOR_LetterHeadNo,
                    "MI_Id": $scope.miid,
                    "HRMD_Id": $scope.HRMD_Id
                };

                apiService.create("FMS_Correspondence/OnChangeLetterHeadNo", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.messageval !== null && promise.messageval !== "") {
                            swal(promise.messageval);
                            $scope.obj.FMSCOR_LetterHeadNo = "";
                        }
                    }
                });
            }
        };

        $scope.saveonfirsttabcorrespondence = function (obj) {
            if ($scope.myForm1.$valid) {

                angular.forEach($scope.deptlist, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid = dd.mI_Id;
                    }
                });

                var clientid = 0;
                var projectid = 0;
                var salesleadid = 0;
                var supplierid = 0;
                $scope.selectedemployees = [];

                if ($scope.obj.FMSCOR_ClientSupplierFlg === "Client") {
                    salesleadid = 0;
                    supplierid = 0;
                    clientid = obj.ismmclT_Id.ismmclT_Id;

                    if (obj.ISMMPR_Id !== undefined && obj.ISMMPR_Id !== null && obj.ISMMPR_Id !== "") {
                        projectid = obj.ISMMPR_Id;
                    } else {
                        projectid = 0;
                    }
                }
                else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Supplier") {
                    salesleadid = 0;
                    supplierid = obj.INVMS_Id.invmS_Id;
                    clientid = 0;
                    projectid = 0;
                    if (obj.ISMMPR_Id !== undefined && obj.ISMMPR_Id !== null && obj.ISMMPR_Id !== "") {
                        projectid = obj.ISMMPR_Id;
                    }
                }
                else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Sales Lead") {
                    salesleadid = obj.ISMSLE_Id.ISMSLE_Id;
                    supplierid = 0;
                    clientid = 0;
                    projectid = 0;
                }
                else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Internal") {

                    angular.forEach($scope.getemployeelist, function (d) {
                        if (d.empck === true) {
                            $scope.selectedemployees.push({ HRME_Id: d.hrmE_Id, FMSCOREMP_Id: d.FMSCOREMP_Id });
                        }
                    });
                }

                var pkid = 0;

                if (obj.FMSCOR_Id > 0) {
                    pkid = obj.FMSCOR_Id;
                }

                obj.FMSCOR_Description = obj.FMSCOR_Description !== undefined && obj.FMSCOR_Description !== null && obj.FMSCOR_Description !== "" ? obj.FMSCOR_Description : "";

                obj.FMSCOR_LetterHeadNo = obj.FMSCOR_LetterHeadNo !== undefined && obj.FMSCOR_LetterHeadNo !== null && obj.FMSCOR_LetterHeadNo !== "" ? obj.FMSCOR_LetterHeadNo : "";

                obj.FMSCOR_ToName = obj.FMSCOR_ToName !== undefined && obj.FMSCOR_ToName !== null && obj.FMSCOR_ToName !== "" ? obj.FMSCOR_ToName : "";

                var data = {
                    "FMSCOR_Id": pkid,
                    "FMSMFC_Id": obj.FMSMFC_Id,
                    "FMSCOR_Subject": obj.FMSCOR_Subject,
                    "FMSCOR_Date": new Date(obj.FMSCOR_Date).toDateString(),
                    "FMSCOR_ClientSupplierFlg": obj.FMSCOR_ClientSupplierFlg,
                    "FMSCOR_RefernceNo": obj.FMSCOR_RefernceNo,
                    "ISMMCLT_Id": clientid,
                    "INVMS_Id": supplierid,
                    "ISMSLE_Id": salesleadid,
                    "ISMMPR_Id": projectid,
                    "FMSCOR_LetterHeadNo": obj.FMSCOR_LetterHeadNo,
                    "FMSCOR_ToName": obj.FMSCOR_ToName,
                    "FMSCOR_Description": obj.FMSCOR_Description,
                    "manualorautorefno": $scope.manualorautorefno,
                    "MI_Id": $scope.miid,
                    "HRMD_Id": $scope.HRMD_Id
                };

                if ($scope.obj.FMSCOR_ClientSupplierFlg === "Internal") {
                    data.employeelist = $scope.selectedemployees;
                }

                apiService.create("FMS_Correspondence/saveonfirsttabcorrespondence", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.saveorupdate === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.saveorupdate === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.saveorupdate === "RefNoError") {
                            swal("Reference No. Not Generated Kindly Contact Administrator");
                        } else if (promise.saveorupdate === "RefNoErrorCodes") {
                            swal("Reference No. Not Generated");
                        } else if (promise.saveorupdate === "Error") {
                            swal("Failed To Save/Update The Record");
                        }
                        else {
                            swal(promise.saveorupdate);
                        }
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                    $scope.OnChangeDept('Tab1');
                });
            }
            else {
                $scope.submitted = true;
                $scope.address = true;
            }
        };

        $scope.Editdatafirsttab = function (userd) {

            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            var data = {
                "FMSCOR_Id": userd.fmscoR_Id,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };
            apiService.create("FMS_Correspondence/Editdatafirsttab", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdatafirsttab_temp = promise.geteditdatafirsttab;

                    if ($scope.geteditdatafirsttab_temp !== null && $scope.geteditdatafirsttab_temp.length > 0) {
                        $scope.geteditdatafirsttab = promise.geteditdatafirsttab;
                        $scope.editable = true;
                        $scope.obj.FMSMFC_Id = $scope.geteditdatafirsttab[0].fmsmfC_Id;
                        $scope.obj.FMSCOR_Id = $scope.geteditdatafirsttab[0].fmscoR_Id;
                        $scope.obj.FMSCOR_Subject = $scope.geteditdatafirsttab[0].fmscoR_Subject;
                        $scope.obj.FMSCOR_LetterHeadNo = $scope.geteditdatafirsttab[0].fmscoR_LetterHeadNo;
                        $scope.obj.FMSCOR_Description = $scope.geteditdatafirsttab[0].fmscoR_Description;
                        $scope.obj.FMSCOR_ToName = $scope.geteditdatafirsttab[0].fmscoR_ToName;
                        $scope.obj.FMSCOR_Date = new Date($scope.geteditdatafirsttab[0].fmscoR_Date);
                        $scope.obj.FMSCOR_ClientSupplierFlg = $scope.geteditdatafirsttab[0].fmscoR_ClientSupplierFlg;
                        $scope.labeldetails = $scope.geteditdatafirsttab[0].fmscoR_ClientSupplierFlg;

                        $scope.obj.FMSCOR_RefernceNo = $scope.geteditdatafirsttab[0].fmscoR_RefernceNo;
                        $scope.manualorautorefno = "Manual";
                        $scope.autorefno = false;

                        if ($scope.obj.FMSCOR_ClientSupplierFlg === "Client") {
                            $scope.geteditclientdata = promise.geteditclientdata;
                            if ($scope.geteditclientdata !== null && $scope.geteditclientdata.length > 0) {
                                $scope.getproject = promise.getproject;
                                $scope.obj.ismmclT_Id = $scope.geteditclientdata[0];
                                console.log($scope.obj.ismmclT_Id);
                                $scope.obj.ISMMPR_Id = $scope.geteditclientdata[0].ismmpR_Id;
                                if ($scope.obj.ISMMPR_Id === null || $scope.obj.ISMMPR_Id === 0) {
                                    $scope.obj.ISMMPR_Id = "";
                                }
                            } else {
                                swal("Client Details Not Found");
                            }
                        }

                        else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Supplier") {
                            $scope.geteditsupplierdata = promise.geteditsupplierdata;
                            if ($scope.geteditsupplierdata !== null && $scope.geteditsupplierdata.length > 0) {
                                $scope.obj.INVMS_Id = $scope.geteditsupplierdata[0];
                                $scope.obj.ISMMPR_Id = $scope.geteditclientdata[0].ismmpR_Id;
                                if ($scope.obj.ISMMPR_Id === null || $scope.obj.ISMMPR_Id === 0) {
                                    $scope.obj.ISMMPR_Id = "";
                                }
                            } else {
                                swal("Supplier Details Not Found");
                            }
                        }

                        else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Sales Lead") {
                            $scope.geteditsalesdata = promise.geteditsalesdata;
                            if ($scope.geteditsalesdata !== null && $scope.geteditsalesdata.length > 0) {
                                $scope.obj.ISMSLE_Id = $scope.geteditsalesdata[0];
                            } else {
                                swal("Sales Lead Details Not Found");
                            }
                        }

                        else if ($scope.obj.FMSCOR_ClientSupplierFlg === "Internal") {

                            $scope.geteditemployeedetails = promise.geteditemployeedetails;

                            angular.forEach($scope.getemployeelist, function (d) {
                                angular.forEach($scope.geteditemployeedetails, function (dd) {
                                    if (d.hrmE_Id === dd.hrmE_Id) {
                                        d.FMSCOREMP_Id = dd.fmscoremP_Id;
                                        d.empck = true;
                                    }
                                });
                            });
                        }
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                }
            });
        };

        $scope.activedeactivefirsttab = function (useer) {

            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            var data = {
                "FMSCOR_Id": useer.fmscoR_Id,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };

            var mgs = "";
            var confirmmgs = "";
            if (useer.fmscoR_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

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
                        apiService.create("FMS_Correspondence/activedeactivefirsttab", data).then(function (promise) {
                            if (promise.messageval === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear_first_tab();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.viewfirsttabdetails = function (us) {
            $scope.getviewclientdata = [];
            $scope.getviewsupplierdata = [];
            $scope.getviewsalesdata = [];
            $scope.fmscoR_ClientSupplierFlg_new = "";
            $scope.referncenodetails = us.fmscoR_RefernceNo;

            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });


            var data = {
                "FMSCOR_Id": us.fmscoR_Id,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/viewfirsttabdetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.fmscoR_ClientSupplierFlg_new = promise.fmscoR_ClientSupplierFlg;
                    if (promise.fmscoR_ClientSupplierFlg === "Client") {
                        $scope.getviewclientdata = promise.getviewclientdata;
                        if ($scope.getviewclientdata === null || $scope.getviewclientdata.length === 0) {
                            swal("No Records Found");
                        }

                    } else if (promise.fmscoR_ClientSupplierFlg === "Supplier") {
                        $scope.getviewsupplierdata = promise.getviewsupplierdata;
                        if ($scope.getviewsupplierdata === null || $scope.getviewsupplierdata.length === 0) {
                            swal("No Records Found");
                        }

                    } else if (promise.fmscoR_ClientSupplierFlg === "Sales Lead") {
                        $scope.getviewsalesdata = promise.getviewsalesdata;
                        if ($scope.getviewsalesdata === null || $scope.getviewsalesdata.length === 0) {
                            swal("No Records Found");
                        }
                    } else if (promise.fmscoR_ClientSupplierFlg === "Internal") {
                        $scope.getviewemployeedetails = promise.getviewemployeedetails;
                        if ($scope.getviewemployeedetails === null || $scope.getviewemployeedetails.length === 0) {
                            swal("No Records Found");
                        }
                    }
                }
            });
        };

        $scope.activedeactivesubdetails = function (dd) {
            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            var data = {
                "details_pkid": dd.detailsid,
                "FMSCOR_Id": dd.fmscoR_Id,
                "FMSCOR_ClientSupplierFlg": $scope.fmscoR_ClientSupplierFlg_new,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };

            var mgs = "";
            var confirmmgs = "";
            if (dd.activeflag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }


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
                        apiService.create("FMS_Correspondence/activedeactivesubdetails", data).then(function (promise) {
                            if (promise.messageval === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }

                            $scope.fmscoR_ClientSupplierFlg_new = promise.fmscoR_ClientSupplierFlg;
                            if (promise.fmscoR_ClientSupplierFlg === "Client") {
                                $scope.getviewclientdata = promise.getviewclientdata;
                                if ($scope.getviewclientdata === null || $scope.getviewclientdata.length === 0) {
                                    swal("No Records Found");
                                }

                            } else if (promise.fmscoR_ClientSupplierFlg === "Supplier") {
                                $scope.getviewsupplierdata = promise.getviewsupplierdata;
                                if ($scope.getviewsupplierdata === null || $scope.getviewsupplierdata.length === 0) {
                                    swal("No Records Found");
                                }

                            } else if (promise.fmscoR_ClientSupplierFlg === "Sales Lead") {
                                $scope.getviewsalesdata = promise.getviewsalesdata;
                                if ($scope.getviewsalesdata === null || $scope.getviewsalesdata.length === 0) {
                                    swal("No Records Found");
                                }
                            }

                            else if (promise.fmscoR_ClientSupplierFlg === "Internal") {
                                $scope.getviewemployeedetails = promise.getviewemployeedetails;
                                if ($scope.getviewemployeedetails === null || $scope.getviewemployeedetails.length === 0) {
                                    swal("No Records Found");
                                }
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.searchrefernceno = function (searchrefno, tab) {
            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            if (searchrefno === null || searchrefno === "") {
                swal("Enter Refernce No. To Search The Details");
                return;
            }
            var data = {
                "FMSCOR_RefernceNo": searchrefno,
                "TabNo": tab,
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/searchrefernceno", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getgriddatatemp = [];
                    $scope.getgriddatatemp = promise.getfirstgriddata;
                    if (tab === "Tab1") {
                        $scope.getfirstgriddata = $scope.getgriddatatemp;
                        if ($scope.getfirstgriddata === null || $scope.getfirstgriddata.length === 0) {
                            swal("No Records Found");
                            $scope.clear_first_tab();
                        }
                    } else if (tab === "Tab2") {
                        $scope.getsecondtabdata = $scope.getgriddatatemp;
                        if ($scope.getsecondtabdata === null || $scope.getsecondtabdata.length === 0) {
                            swal("No Records Found");
                            $scope.clear_second_tab();
                        }
                    } else if (tab === "Tab3") {
                        $scope.getthirdtabdata = $scope.getgriddatatemp;
                        if ($scope.getthirdtabdata === null || $scope.getthirdtabdata.length === 0) {
                            swal("No Records Found");
                            $scope.clear_third_tab();
                        }
                    }
                }
            });
        };

        $scope.clear_first_tab = function (data) {
            $scope.obj.FMSMFC_Id = "";
            $scope.maxdFMSCOR_Date = new Date($scope.today);
            $scope.obj.FMSCOR_Subject = "";
            $scope.obj.FMSCOR_Description = "";
            $scope.obj.FMSCOR_LetterHeadNo = "";
            $scope.obj.FMSCOR_Date = new Date($scope.today);
            $scope.obj.FMSCOR_ClientSupplierFlg = "";
            $scope.obj.FMSCOR_RefernceNo = "";
            $scope.obj.ismmclT_Id = "";
            $scope.obj.ISMMPR_Id = "";
            $scope.obj.INVMS_Id = "";
            $scope.obj.ISMSLE_Id = "";
            $scope.obj.searchrefno = "";
            $scope.obj.FMSCOR_Id = 0;
            $scope.getfirstgriddata = [];
            $scope.getemployeelist = [];
            $scope.labeldetails = "Client";
            $scope.obj.FMSCOR_ClientSupplierFlg = 'Client';
            $scope.obj.FMSCOR_ToName = "";
            $scope.editable = false;
            $scope.submitted = false;

            $scope.onfirsttabcorrespondence();
        };

        $scope.clear_first_tab_load = function (data) {
            $scope.obj.FMSMFC_Id = "";
            $scope.obj.FMSCOR_Subject = "";
            $scope.maxdFMSCOR_Date = new Date($scope.today);
            $scope.obj.FMSCOR_Description = "";
            $scope.obj.FMSCOR_LetterHeadNo = "";
            $scope.obj.FMSCOR_Date = new Date($scope.today);
            $scope.obj.FMSCOR_ClientSupplierFlg = "";
            $scope.obj.FMSCOR_ToName = "";
            $scope.obj.ismmclT_Id = "";
            $scope.obj.ISMMPR_Id = "";
            $scope.obj.INVMS_Id = "";
            $scope.obj.ISMSLE_Id = "";
            $scope.obj.searchrefno = "";
            $scope.obj.FMSCOR_Id = 0;
            $scope.getfirstgriddata = [];
            $scope.getemployeelist = [];
            $scope.labeldetails = "Client";
            $scope.obj.FMSCOR_ClientSupplierFlg = 'Client';
            $scope.editable = false;
            $scope.submitted = false;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getemployeelist.some(function (options) {
                return options.empck;
            });
        };

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.hrmE_EmployeeName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.getemployeelist, function (uem) {
                uem.empck = toggleStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.checkall = $scope.getemployeelist.every(function (options) {
                return options.empck;
            });
        };


        /* On Change Department */
        $scope.OnChangeDept = function (tabname) {
            $scope.autorefno = true;
            angular.forEach($scope.deptlist, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid = dd.mI_Id;
                }
            });

            if (tabname === "Tab1") {
                $scope.clear_first_tab_load();
                $scope.getfirstgriddata = [];
                $scope.getemployeelist = [];
                $scope.getclient = [];
                $scope.getsaleslead = [];
                $scope.getsuppliers = [];
                $scope.getproject = [];
                $scope.getfilecategory = [];
                $scope.client_Supplier_SalesLead_code = "";
                $scope.maxdFMSCOR_Date = new Date($scope.today);
                $scope.obj.FMSCOR_Date = new Date($scope.today);
            }

            else if (tabname === "Tab2") {
                $scope.clear_second_tab_load();
                $scope.getsecondtabdata = [];
                $scope.getclientfiles = [];
                $scope.getsuppliersfiles = [];
                $scope.getsalesleadfiles = [];
                $scope.getfilecategoryfiles = [];
            }
            else if (tabname === "Tab3") {
                $scope.clear_third_tab_load();
                $scope.getthirdtabdata = [];
            }

            var data = {
                "MI_Id": $scope.miid,
                "HRMD_Id": $scope.HRMD_Id,
                "TabNo": tabname
            };
            apiService.create("FMS_Correspondence/OnChangeDept", data).then(function (promise) {
                if (tabname === "Tab1") {
                    $scope.labeldetails = "Client";
                    $scope.obj.FMSCOR_ClientSupplierFlg = 'Client';
                    if (promise.messageval === "NoPrivileges") {
                        swal("Department Is Not Mapped For This User");
                    } else if (promise.messageval === "NotMapped") {
                        swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                    } else if (promise.messageval === "Mapped") {
                        $scope.getfilecategory = promise.getfilecategory;
                        $scope.getclient = promise.getclient;
                        $scope.getsaleslead = promise.getsaleslead;
                        $scope.getsuppliers = promise.getsuppliers;
                        $scope.getfirstgriddata = promise.getfirstgriddata;
                        $scope.getemployeelist = promise.getemployeelist;
                        $scope.getproject = promise.getproject;

                        angular.forEach($scope.getemployeelist, function (d) {
                            d.FMSCOREMP_Id = 0;
                        });

                        if (promise.getgeneralconfig !== null && promise.getgeneralconfig.length > 0) {
                            $scope.FMSManualReferenceFlg = promise.getgeneralconfig[0].ivrmgC_FMSManualReferenceFlg;
                            $scope.savebtn = false;
                            if ($scope.FMSManualReferenceFlg === true) {
                                $scope.manualorautorefno = "Auto";
                                $scope.autorefno = true;
                            }
                            else if ($scope.FMSManualReferenceFlg === false) {
                                $scope.manualorautorefno = "Manual";
                                $scope.autorefno = false;
                            } else {
                                $scope.savebtn = true;
                                swal("Map The Auto/Manual Reference No. In General Settings");
                            }

                        } else {
                            swal("General Setting Not Done For This Institution");
                        }
                    } else {
                        swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                    }
                }

                else if (tabname === "Tab2") {

                    $scope.objfiles.From_FMSCOR_Date = new Date(promise.fromdate);
                    $scope.objfiles.To_FMSCOR_Date = new Date($scope.today);
                    $scope.todate2 = new Date($scope.today);

                    if (promise.messageval === "NoPrivileges") {
                        swal("Department Is Not Mapped For This User");
                    } else if (promise.messageval === "NotMapped") {
                        swal("No Physical File Is Mapped For Your Department , Kindly Contact Administrator");
                    } else if (promise.messageval === "Mapped") {

                        $scope.getphysicalfilelist = promise.getphysicalfilelist;
                        $scope.getcabinetlist = promise.getcabinetlist;
                        $scope.getsecondtabdata = promise.getsecondtabdata;

                        //$scope.searchreferncenosecondtab($scope.objfiles);

                        $timeout(function () { $scope.searchreferncenosecondtab($scope.objfiles); }, 1000);
                    } else {
                        swal("No Physical File Is Mapped For Your Department , Kindly Contact Administrator");
                    }
                }

                else if (tabname === "Tab3") {

                    $scope.objmode.From_FMSCOR_Date = new Date(promise.fromdate);
                    $scope.objmode.To_FMSCOR_Date = new Date($scope.today);
                    $scope.todate3 = new Date($scope.today);

                    if (promise.messageval === "NoPrivileges") {
                        swal("Department Is Not Mapped For This User");
                    } else if (promise.messageval === "NotMapped") {
                        swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                    } else if (promise.messageval === "Mapped") {

                        $scope.getdispatchmode = promise.getdispatchmode;
                        $scope.getthirdtabdata = promise.getthirdtabdata;

                        angular.forEach($scope.getthirdtabdata, function (dd) {
                            if (dd.fmscormD_FilePath !== null && dd.fmscormD_FilePath !== '') {
                                var img = dd.fmscormD_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.fmscormD_FilePath;
                                }
                            }
                        });
                       // $scope.searchreferncenothirdtab($scope.objmode);
                        $timeout(function () { $scope.searchreferncenothirdtab($scope.objmode); }, 1000);
                    } else {
                        swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                    }

                }
            });
        };

        /* SECOND TAB */
        $scope.onchnageradiobtnsfile = function () {
            $scope.objfiles.ismmclT_Id = "";
            $scope.objfiles.INVMS_Id = "";
            $scope.objfiles.ISMSLE_Id = "";
            $scope.labeldetailsfiles = "";
            $scope.labeldetailsfiles = $scope.objfiles.ClientSupplierFlgflag;
        };

        $scope.onsecondtabcorrespondence = function (dd) {
            $scope.getreferencelist = [];
            $scope.getphysicalfilelist = [];
            // $scope.getreferncenolist = [];
            $scope.clear_second_tab_load(dd);
            var pageid = 2;
            $scope.labeldetailsfiles = "Client";
            $scope.submitted2 = false;
            $scope.HRMD_Id = "";
            apiService.getURI("FMS_Correspondence/onsecondtabcorrespondence", pageid).then(function (promise) {

                $scope.objfiles.From_FMSCOR_Date = new Date(promise.fromdate);
                $scope.objfiles.To_FMSCOR_Date = new Date($scope.today);
                $scope.todate2 = new Date($scope.today);

                if (promise.messageval === "NoPrivileges") {
                    swal("Department Is Not Mapped For This User");
                } else if (promise.messageval === "NotMapped") {
                    swal("No Physical File Name Is Mapped For Your Department , Kindly Contact Administrator");
                } else if (promise.messageval === "Mapped") {
                    $scope.getphysicalfilelist = promise.getphysicalfilelist;
                    $scope.getcabinetlist = promise.getcabinetlist;
                    $scope.getsecondtabdata = promise.getsecondtabdata;
                    $scope.deptlist2 = promise.institutiondeptdetails;
                    $scope.HRMD_Id = promise.hrmD_Id;
                    //$scope.searchreferncenosecondtab($scope.objfiles);

                    $timeout(function () { $scope.searchreferncenosecondtab($scope.objfiles); }, 1000);

                } else {
                    swal("No Physical File Name Is Mapped For Your Department , Kindly Contact Administrator");
                }
            });
        };

        $scope.searchreferncenosecondtab = function (obj2) {
            if ($scope.myForm2.$valid) {
                $scope.getreferncenolist = [];
                $scope.getreferncedetails = [];

                $scope.details = "";
                $scope.objfiles.FMSCOR_Id = "";
                $scope.refno = "";
                $scope.refnodate = "";
                $scope.refnosubject = "";
                $scope.refnodesc = "";
                $scope.refnotowhom = "";
                $scope.refnoletterno = "";
                $scope.fmsfilesupload = [];
                $scope.fmsfilesupload = [{ id: 'fmscore1' }];

                angular.forEach($scope.deptlist2, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid2 = dd.mI_Id;
                    }
                });
                $scope.flagtypes = [];
                if ($scope.objfiles.ClientSupplierFlgflag === "All") {
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Client" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Sales Lead" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Supplier" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Others" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Internal" });
                } else {
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: $scope.objfiles.ClientSupplierFlgflag });
                }

                var data = {
                    "FMSCOR_ClientSupplierFlg": obj2.ClientSupplierFlgflag,
                    "fromdate": new Date(obj2.From_FMSCOR_Date).toDateString(),
                    "todate": new Date(obj2.To_FMSCOR_Date).toDateString(),
                    "MI_Id": $scope.miid2,
                    "flagtypes": $scope.flagtypes,
                    "HRMD_Id": $scope.HRMD_Id

                };
                apiService.create("FMS_Correspondence/searchreferncenosecondtab", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.submitted2 = false;
                        $scope.getreferncenolist = promise.getreferncenolist;
                        if ($scope.getreferncenolist === null || $scope.getreferncenolist.length === 0) {
                            swal("No Reference No. List Found For Selected Details");
                        }
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.onchangerefnosecondtab = function (objrefno) {
            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";
            $scope.getviewemployeedetails = [];
            $scope.fmsfilesupload = [];
            $scope.fmsfilesupload = [{ id: 'fmscore1' }];
            $scope.objfiles.FMSMPFN_Id = "";
            $scope.objfiles.FMSMFCBT_Id = "";
            $scope.objfiles.FMSMFCR_Id = "";
            $scope.objfiles.FMSMFCC_Id = "";

            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });
            var data = {
                "FMSCOR_Id": objrefno.fmscoR_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/onchangerefnosecondtab", data).then(function (promise) {
                if (promise !== null) {
                    $scope.fmsfilesupload = promise.getfilesdetails;

                    $scope.getrefnodetails = promise.getrefnodetails;

                    $scope.getreferncedetails = promise.getreferncedetails;

                    $scope.details = $scope.getreferncedetails[0].Details;
                    $scope.refno = $scope.getreferncedetails[0].FMSCOR_RefernceNo;
                    $scope.refnodate = $scope.getreferncedetails[0].FMSCOR_Date;
                    $scope.refnosubject = $scope.getreferncedetails[0].FMSCOR_Subject;
                    $scope.refnodesc = $scope.getreferncedetails[0].FMSCOR_Description;
                    $scope.refnotowhom = $scope.getreferncedetails[0].FMSCOR_ToName;
                    $scope.refnoletterno = $scope.getreferncedetails[0].FMSCOR_LetterHeadNo;
                    $scope.refnoflag = $scope.getreferncedetails[0].FMSCOR_ClientSupplierFlg;
                    $scope.refnoid = $scope.getreferncedetails[0].FMSCOR_Id;

                    if ($scope.refnoflag === "Internal") {
                        $scope.getviewemployeedetails = promise.getviewemployeedetails;
                    }

                    if ($scope.fmsfilesupload !== null && $scope.fmsfilesupload.length > 0) {

                        angular.forEach($scope.fmsfilesupload, function (ddd) {

                            var img = ddd.fmscorfL_SoftCopyFilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            console.log("ddd.filetype : " + ddd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.fmscorfL_SoftCopyFilePath;
                            }
                        });
                    } else {
                        $scope.fmsfilesupload = [];
                        $scope.fmsfilesupload = [{ id: 'fmscore1' }];
                    }
                }
            });
        };

        $scope.ViewEmployeeDetails = function () {
            $('#mymodalviewdetailsemployeedetails').modal('show');
        };

        $scope.onchangefilecabinet = function () {
            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });
            var data = {
                "FMSMFCBT_Id": $scope.objfiles.FMSMFCBT_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id
            };
            apiService.create("FMS_Correspondence/onchangefilecabinet", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getrows = promise.getrows;
                    $scope.getcolumns = promise.getcolumns;
                }
            });
        };

        $scope.savesecondtabcorrespondence = function (objfiles2) {
            if ($scope.myForm21.$valid) {

                angular.forEach($scope.deptlist2, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid2 = dd.mI_Id;
                    }
                });

                var id2 = 0;
                if (objfiles2.FMSCORFL_Id > 0) {
                    id2 = objfiles2.FMSCORFL_Id;
                } else {
                    id2 = 0;
                }
                var data = {
                    "FMSCORFL_Id": id2,
                    "FMSCOR_Id": objfiles2.FMSCOR_Id.fmscoR_Id,
                    "FMSMPFN_Id": objfiles2.FMSMPFN_Id.fmsmpfN_Id,
                    "FMSMFCBT_Id": objfiles2.FMSMFCBT_Id,
                    "FMSMFCR_Id": objfiles2.FMSMFCR_Id,
                    "FMSMFCC_Id": objfiles2.FMSMFCC_Id,
                    "FMS_Temp_UploadFiles": $scope.fmsfilesupload,
                    "MI_Id": $scope.miid2,
                    "HRMD_Id": $scope.HRMD_Id
                };

                apiService.create("FMS_Correspondence/savesecondtabcorrespondence", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.saveorupdate === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved /Updated Successfully");
                            } else {
                                swal("Failed To Save/ Update Record");
                            }
                        } else if (promise.saveorupdate === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update The Record");
                        }

                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }

                    $scope.OnChangeDept('Tab2');
                });
            }
            else {
                $scope.submitted21 = true;
            }
        };

        $scope.Editdatasecondtab = function (userd2) {
            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });

            $scope.flagtypes = [];
            if ($scope.objfiles.ClientSupplierFlgflag === "All") {
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Client" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Sales Lead" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Supplier" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Others" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Internal" });
            } else {
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: $scope.objfiles.ClientSupplierFlgflag });
            }

            var data = {
                "FMSCORFL_Id": userd2.fmscorfL_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id,
                "flagtypes": $scope.flagtypes
            };

            apiService.create("FMS_Correspondence/Editdatasecondtab", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.messageval !== "Error") {

                        if (promise.getsecondeditdata !== null && promise.getsecondeditdata.length > 0) {
                            $scope.editableflag = true;
                            $scope.getsecondeditdata = promise.getsecondeditdata;
                            $scope.filenamedetails = promise.filenamedetails;

                            $scope.objfiles.FMSMPFN_Id = $scope.filenamedetails[0];

                            //$scope.objfiles.FMSMFCBT_Id = $scope.getsecondeditdata[0].fmsmfcbT_Id;

                            //$scope.getrows = promise.getrows;
                            //$scope.getcolumns = promise.getcolumns;

                            //$scope.objfiles.FMSMFCR_Id = $scope.getsecondeditdata[0].fmsmfcR_Id;
                            //$scope.objfiles.FMSMFCC_Id = $scope.getsecondeditdata[0].fmsmfcC_Id;

                            if (promise.checkcabinetdetails !== null && promise.checkcabinetdetails.length > 0) {

                                $scope.checkcabinetdetails = promise.checkcabinetdetails;
                                $scope.getcabinetlist = promise.getcabinetlist;
                                $scope.getrows = promise.getrows;
                                $scope.getcolumns = promise.getcolumns;

                                $scope.objfiles.FMSMFCBT_Id = $scope.checkcabinetdetails[0].fmsmfcbT_Id;
                                $scope.objfiles.FMSMFCR_Id = $scope.checkcabinetdetails[0].fmsmfcR_Id;
                                $scope.objfiles.FMSMFCC_Id = $scope.checkcabinetdetails[0].fmsmfcC_Id;


                            } else {
                                swal("Cabinet Details Not Mapped For Selected Physical File Name");
                            }

                            $scope.objfiles.FMSCORFL_Id = $scope.getsecondeditdata[0].fmscorfL_Id;


                            if (promise.getfmcoriddetails !== null && promise.getfmcoriddetails.length > 0) {

                                $scope.labeldetailsfiles = promise.getfmcoriddetails[0].fmscoR_ClientSupplierFlg;

                                $scope.objfiles.From_FMSCOR_Date = new Date(promise.getfmcoriddetails[0].fmscoR_Date);
                                $scope.objfiles.To_FMSCOR_Date = new Date(promise.getfmcoriddetails[0].fmscoR_Date);
                                //$scope.objfiles.FMSMFC_Id = promise.getfmcoriddetails[0].fmsmfC_Id;

                                $scope.getreferncenolist = promise.getreferncenolist;
                                $scope.getrefnodetails = promise.getreferncenolist;

                                angular.forEach($scope.getreferncenolist, function (dd) {
                                    if (dd.fmscoR_Id === promise.getfmcoriddetails[0].fmscoR_Id) {
                                        $scope.objfiles.FMSCOR_Id = dd;
                                    }
                                });


                                $scope.fmsfilesupload = promise.getfilesdetails;

                                if ($scope.fmsfilesupload !== null && $scope.fmsfilesupload.length > 0) {

                                    angular.forEach($scope.fmsfilesupload, function (ddd) {

                                        var img = ddd.fmscorfL_SoftCopyFilePath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        ddd.filetype = lastelement;
                                        console.log("ddd.filetype : " + ddd.filetype);
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.fmscorfL_SoftCopyFilePath;
                                        }
                                    });
                                } else {
                                    $scope.fmsfilesupload = [];
                                    $scope.fmsfilesupload = [{ id: 'fmscore1' }];
                                }

                                $scope.getreferncedetails = promise.getreferncedetails;

                                $scope.details = $scope.getreferncedetails[0].Details;
                                $scope.refno = $scope.getreferncedetails[0].FMSCOR_RefernceNo;
                                $scope.refnodate = $scope.getreferncedetails[0].FMSCOR_Date;
                                $scope.refnosubject = $scope.getreferncedetails[0].FMSCOR_Subject;
                                $scope.refnodesc = $scope.getreferncedetails[0].FMSCOR_Description;
                                $scope.refnotowhom = $scope.getreferncedetails[0].FMSCOR_ToName;
                                $scope.refnoletterno = $scope.getreferncedetails[0].FMSCOR_LetterHeadNo;
                                $scope.refnoflag = $scope.getreferncedetails[0].FMSCOR_ClientSupplierFlg;
                                $scope.refnoid = $scope.getreferncedetails[0].FMSCOR_Id;


                                if ($scope.refnoflag === "Internal") {
                                    $scope.getviewemployeedetails = promise.getviewemployeedetails;
                                }



                                //if (promise.getfmcoriddetails[0].fmscoR_ClientSupplierFlg === "Client") {
                                //    $scope.geteditclientdata = promise.geteditclientdata;
                                //    if ($scope.geteditclientdata !== null && $scope.geteditclientdata.length > 0) {
                                //        $scope.objfiles.ismmclT_Id = $scope.geteditclientdata[0];
                                //    } else {
                                //        swal("Client Details Not Found");
                                //    }
                                //}

                                //else if (promise.getfmcoriddetails[0].fmscoR_ClientSupplierFlg === "Supplier") {
                                //    $scope.geteditsupplierdata = promise.geteditsupplierdata;
                                //    if ($scope.geteditsupplierdata !== null && $scope.geteditsupplierdata.length > 0) {
                                //        $scope.objfiles.INVMS_Id = $scope.geteditsupplierdata[0];
                                //    } else {
                                //        swal("Supplier Details Not Found");
                                //    }
                                //}

                                //else if (promise.getfmcoriddetails[0].fmscoR_ClientSupplierFlg === "Sales Lead") {
                                //    $scope.geteditsalesdata = promise.geteditsalesdata;
                                //    if ($scope.geteditsalesdata !== null && $scope.geteditsalesdata.length > 0) {
                                //        $scope.objfiles.ISMSLE_Id = $scope.geteditsalesdata[0];
                                //    } else {
                                //        swal("Sales Lead Details Not Found");
                                //    }
                                //}
                            }
                        }
                    }
                }
            });
        };

        $scope.activedeactivesecondtab = function (useer) {
            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });

            var data = {
                "FMSCORFL_Id": useer.fmscorfL_Id,
                "FMSCOR_Id": useer.fmscoR_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id
            };

            var mgs = "";
            var confirmmgs = "";
            if (useer.fmscorfL_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

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
                        apiService.create("FMS_Correspondence/activedeactivesecondtab", data).then(function (promise) {
                            if (promise.messageval === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear_second_tab();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.clear_second_tab_load = function (dd) {
            $scope.submitted2 = false;
            $scope.submitted21 = false;
            $scope.objfiles.ismmclT_Id = "";
            $scope.objfiles.ISMMPR_Id = "";
            $scope.objfiles.INVMS_Id = "";
            $scope.objfiles.ISMSLE_Id = "";
            $scope.objfiles.FMSCOR_Id = "";
            $scope.objfiles.FMSMFC_Id = "";

            $scope.getrows = [];
            $scope.getcolumns = [];
            $scope.geteditsalesdata = [];
            $scope.geteditclientdata = [];
            $scope.geteditsupplierdata = [];
            $scope.getrefnodetails = [];
            $scope.getreferncedetails = [];
            $scope.fmsfilesupload = [];
            $scope.fmsfilesupload = [{ id: 'fmscore1' }];
            $scope.labeldetailsfiles = "Client";
            $scope.fromdate2 = new Date($scope.today);
            $scope.todate2 = new Date($scope.today);

            if (dd !== "Second") {
                $scope.getreferncenolist = [];
                $scope.objfiles.From_FMSCOR_Date = null;
                $scope.objfiles.To_FMSCOR_Date = null;
            }

            $scope.objfiles.FMSMPFN_Id = "";
            $scope.objfiles.FMSMFCBT_Id = "";
            $scope.objfiles.FMSMFCR_Id = "";
            $scope.objfiles.FMSMFCC_Id = "";
            $scope.objfiles.searchrefnotab2 = "";
            $scope.editableflag = false;

            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";

        };

        $scope.clear_second_tab = function (data) {
            $scope.submitted2 = false;
            $scope.submitted21 = false;
            $scope.objfiles.ismmclT_Id = "";
            $scope.objfiles.ISMMPR_Id = "";
            $scope.objfiles.INVMS_Id = "";
            $scope.objfiles.ISMSLE_Id = "";
            $scope.objfiles.FMSCOR_Id = "";
            $scope.objfiles.FMSMFC_Id = "";
            //$scope.getreferncenolist = [];
            $scope.geteditsalesdata = [];
            $scope.geteditclientdata = [];
            $scope.geteditsupplierdata = [];
            $scope.getrows = [];
            $scope.getcolumns = [];
            $scope.fmsfilesupload = [];
            $scope.fmsfilesupload = [{ id: 'fmscore1' }];
            $scope.labeldetailsfiles = "Client";
            $scope.fromdate2 = new Date($scope.today);
            $scope.todate2 = new Date($scope.today);
            $scope.objfiles.FMSMPFN_Id = "";
            $scope.objfiles.FMSMFCBT_Id = "";
            $scope.objfiles.FMSMFCR_Id = "";
            $scope.objfiles.FMSMFCC_Id = "";
            $scope.objfiles.searchrefnotab2 = "";
            $scope.editableflag = false;
            $scope.getrefnodetails = [];
            $scope.getreferncedetails = [];
            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";
            $scope.valuesecondtab = "Second";
            $scope.onsecondtabcorrespondence($scope.valuesecondtab);
        };

        $scope.OnChangeFromDateFile = function () {
            $scope.objfiles.To_FMSCOR_Date = null;
            $scope.todate2 = new Date($scope.today);

            var date = new Date($scope.objfiles.From_FMSCOR_Date);
            var year = date.getFullYear();
            var month = date.getMonth();            
            var lastDay = new Date(year, month + 1, 0);
            $scope.objfiles.To_FMSCOR_Date = lastDay;

            if ($scope.objfiles.To_FMSCOR_Date > $scope.todate2) {
                $scope.todate2 = new Date($scope.today);
                $scope.objfiles.To_FMSCOR_Date = new Date($scope.today);
            }

            $scope.searchreferncenosecondtab($scope.objfiles);
        };

        $scope.OnChangeToDateFile = function (objfiles) {
            $scope.searchreferncenosecondtab(objfiles);
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };

        $scope.interacted21 = function (field) {
            return $scope.submitted21;
        };

        $scope.ViewCorrespondanceFiles = function (dd) {
            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });
            $scope.referncenodetails = dd.fmscoR_RefernceNo;
            var data = {
                "FMSCOR_Id": dd.fmscoR_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/ViewCorrespondanceFiles", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getviewfile = promise.getviewfile;

                    if ($scope.getviewfile !== null && $scope.getviewfile.length > 0) {
                        angular.forEach($scope.getviewfile, function (dd) {
                            var img = dd.fmscorfL_SoftCopyFilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.fmscorfL_SoftCopyFilePath;
                            }
                        });
                        $('#mymodalviewfiles').modal('show');
                    } else {
                        swal("No Files Found");
                    }
                }
            });
        };

        $scope.DeleteCorrespondanceFile = function (dd) {
            var data = {
                "FMSCORFLSC_Id": dd.fmscorflsC_Id,
                "FMSCOR_Id": dd.fmscoR_Id
            };
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FMS_Correspondence/DeleteCorrespondanceFile", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted Successfully");
                            } else {
                                swal("Failed To Delete Record");
                            }
                            $scope.getviewfile = promise.getviewfile;

                            if ($scope.getviewfile !== null && $scope.getviewfile.length > 0) {
                                angular.forEach($scope.getviewfile, function (dd) {
                                    var img = dd.fmscorfL_SoftCopyFilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.fmscorfL_SoftCopyFilePath;
                                    }
                                });
                            } else {
                                swal("No Files Found");
                                //$s('#mymodalviewfiles').modal('hide');
                            }

                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.fmsfilesupload = [{ id: 'fmscore1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.fmsfilesupload.length + 1;
            if (newItemNo <= 50) {
                $scope.fmsfilesupload.push({ 'id': 'fmscore' + newItemNo });
            }
            console.log($scope.fmsfilesupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.fmsfilesupload.length - 1;
            $scope.fmsfilesupload.splice(index, 1);

            if ($scope.fmsfilesupload.length === 0) {
                //data
            }
        };

        $scope.sort1 = function (key) {
            $scope.sortReverse1 = ($scope.sortKey1 === key) ? !$scope.sortReverse1 : $scope.sortReverse1;
            $scope.sortKey1 = key;
        };

        $scope.OnChangephysicalpath = function () {
            angular.forEach($scope.deptlist2, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });
            var data = {
                "FMSMPFN_Id": $scope.objfiles.FMSMPFN_Id.fmsmpfN_Id,
                "MI_Id": $scope.miid2
            };

            apiService.create("FMS_Correspondence/OnChangephysicalpath", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.checkcabinetdetails !== null && promise.checkcabinetdetails.length > 0) {

                        $scope.checkcabinetdetails = promise.checkcabinetdetails;
                        $scope.getcabinetlist = promise.getcabinetlist;
                        $scope.getrows = promise.getrows;
                        $scope.getcolumns = promise.getcolumns;

                        $scope.objfiles.FMSMFCBT_Id = $scope.checkcabinetdetails[0].fmsmfcbT_Id;
                        $scope.objfiles.FMSMFCR_Id = $scope.checkcabinetdetails[0].fmsmfcR_Id;
                        $scope.objfiles.FMSMFCC_Id = $scope.checkcabinetdetails[0].fmsmfcC_Id;


                    } else {
                        swal("Cabinet Details Not Mapped For Selected Physical File Name");
                    }
                }
            });
        };

        $scope.uploadfmsfilesdocuments1 = [];

        $scope.uploadfmsdoc = function (input, document) {
            $scope.uploadfmsfilesdocuments1 = input.files;
            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
                {
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
                } else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadfmsfilesdocuments1.length; i++) {
                formData.append("File", $scope.uploadfmsfilesdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/fmsdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.fmscorfL_SoftCopyFilePath = d;
                    data.fmscorfL_SoftCopyFileName = $scope.filename;
                    $('#').attr('src', data.fmscorfL_SoftCopyFilePath);
                    var img = data.fmscorfL_SoftCopyFilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.fmscorfL_SoftCopyFilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        /* THIRD TAB */
        $scope.onchnageradiobtnsmode = function () {
            $scope.objmode.ismmclT_Id = "";
            $scope.objmode.INVMS_Id = "";
            $scope.objmode.ISMSLE_Id = "";
            $scope.labeldetailsmode = "";
            $scope.labeldetailsmode = $scope.objmode.ClientSupplierFlgmode;
        };

        $scope.OnChangeFromDateMode = function () {
            $scope.objmode.To_FMSCOR_Date = null;
            $scope.todate3 = new Date($scope.today);

            var date = new Date($scope.objmode.From_FMSCOR_Date);
            var year = date.getFullYear();
            var month = date.getMonth();           
            var lastDay = new Date(year, month + 1, 0);            
            $scope.objmode.To_FMSCOR_Date = lastDay;

            if ($scope.objmode.To_FMSCOR_Date > $scope.todate3) {
                $scope.todate3 = new Date($scope.today);
                $scope.objmode.To_FMSCOR_Date = new Date($scope.today);
            }
            $scope.searchreferncenothirdtab(objmode);
        };

        $scope.OnChangeToDateMode = function (objmode) {
            $scope.searchreferncenothirdtab(objmode);
        };

        $scope.onthirdtabcorrespondence = function (dd) {
            $scope.getreferencelist = [];
            var pageid = 2;
            $scope.clear_third_tab_load(dd);
            $scope.submitted3 = false;
            $scope.HRMD_Id = "";
            apiService.getURI("FMS_Correspondence/onthirdtabcorrespondence", pageid).then(function (promise) {
                if (promise.messageval === "NoPrivileges") {
                    swal("Department Is Not Mapped For This User");
                } else if (promise.messageval === "NotMapped") {
                    swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                } else if (promise.messageval === "Mapped") {
                    $scope.objmode.From_FMSCOR_Date = new Date(promise.fromdate);
                    $scope.objmode.To_FMSCOR_Date = new Date($scope.today);
                    $scope.todate3 = new Date($scope.today);

                    $scope.getdispatchmode = promise.getdispatchmode;
                    $scope.getthirdtabdata = promise.getthirdtabdata;
                    $scope.deptlist3 = promise.institutiondeptdetails;
                    $scope.HRMD_Id = promise.hrmD_Id;
                    angular.forEach($scope.getthirdtabdata, function (dd) {
                        if (dd.fmscormD_FilePath !== null && dd.fmscormD_FilePath !== '') {
                            var img = dd.fmscormD_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.fmscormD_FilePath;
                            }
                        }
                    });
                    //$scope.searchreferncenothirdtab($scope.objmode);
                    $timeout(function () { $scope.searchreferncenothirdtab($scope.objmode); }, 1000);
                } else {
                    swal("No Category Is Mapped For Your Department , Kindly Contact Administrator");
                }
            });
        };

        $scope.searchreferncenothirdtab = function (obj3) {
            if ($scope.myForm3.$valid) {

                $scope.getreferncenolistmode = [];
                $scope.getreferncedetails = [];
                $scope.details = "";
                $scope.refno = "";
                $scope.refnodate = "";
                $scope.refnosubject = "";
                $scope.refnodesc = "";
                $scope.refnotowhom = "";
                $scope.refnoletterno = "";
                $scope.objmode.FMSCOR_Id = "";

                angular.forEach($scope.deptlist3, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid2 = dd.mI_Id;
                    }
                });
                $scope.flagtypes = [];
                if ($scope.objfiles.ClientSupplierFlgflag === "All") {
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Client" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Sales Lead" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Supplier" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Others" });
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Internal" });
                } else {
                    $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: $scope.objfiles.ClientSupplierFlgflag });
                }

                $scope.objmode.FMSMDMD_Id = "";
                $scope.objmode.FMSCORMD_ReferenceNo = "";
                $scope.objmode.FMSCOR_Id = "";
                $scope.getreferncenolistmode = [];
                $scope.fmsmodeupload = [];
                $scope.fmsmodeupload = [{ id: 'fmsmodecore1' }];
                $scope.objmode.FMSCORMD_SentDate = null;

                //var clientidflies = 0;
                //var salesleadidfiles = 0;
                //var supplieridfiles = 0;
                //if (obj3.ClientSupplierFlgmode === "Client") {
                //    salesleadidfiles = 0;
                //    supplieridfiles = 0;
                //    clientidflies = obj3.ismmclT_Id.ismmclT_Id;
                //}
                //else if (obj3.ClientSupplierFlgmode === "Supplier") {
                //    salesleadidfiles = 0;
                //    supplieridfiles = obj3.INVMS_Id.invmS_Id;
                //    clientidflies = 0;
                //}
                //else if (obj3.ClientSupplierFlgmode === "Sales Lead") {
                //    salesleadidfiles = obj3.ISMSLE_Id.ismslE_Id;
                //    supplieridfiles = 0;
                //    clientidflies = 0;
                //}

                var data = {
                    "FMSCOR_ClientSupplierFlg": obj3.ClientSupplierFlgmode,
                    //"ISMMCLT_Id": clientidflies,
                    //"INVMS_Id": supplieridfiles,
                    //"ISMSLE_Id": salesleadidfiles,
                    "fromdate": new Date(obj3.From_FMSCOR_Date).toDateString(),
                    "todate": new Date(obj3.To_FMSCOR_Date).toDateString(),
                    "MI_Id": $scope.miid2,
                    "flagtypes": $scope.flagtypes,
                    "HRMD_Id": $scope.HRMD_Id
                    //"FMSMFC_Id": obj3.FMSMFC_Id

                };
                apiService.create("FMS_Correspondence/searchreferncenosecondtab", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.submitted2 = false;
                        $scope.getreferncenolistmode = promise.getreferncenolist;
                        if ($scope.getreferncenolistmode === null || $scope.getreferncenolistmode.length === 0) {
                            swal("No Reference No. List Found For Selected Details");
                        }
                    }
                });
            } else {
                $scope.submitted3 = true;
            }
        };

        $scope.onchangerefnothirdtab = function (objrefno) {

            $scope.getreferncedetails = [];
            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";
            $scope.getviewemployeedetails = [];

            angular.forEach($scope.deptlist3, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid3 = dd.mI_Id;
                }
            });
            var data = {
                "FMSCOR_Id": objrefno.fmscoR_Id,
                "MI_Id": $scope.miid3,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Correspondence/onchangerefnothirdtab", data).then(function (promise) {
                if (promise !== null) {

                    $scope.getrefnodetails = promise.getrefnodetails;

                    $scope.getreferncedetails = promise.getreferncedetails;

                    $scope.details = $scope.getreferncedetails[0].Details;
                    $scope.refno = $scope.getreferncedetails[0].FMSCOR_RefernceNo;
                    $scope.refnodate = $scope.getreferncedetails[0].FMSCOR_Date;
                    $scope.refnosubject = $scope.getreferncedetails[0].FMSCOR_Subject;
                    $scope.refnodesc = $scope.getreferncedetails[0].FMSCOR_Description;
                    $scope.refnotowhom = $scope.getreferncedetails[0].FMSCOR_ToName;
                    $scope.refnoletterno = $scope.getreferncedetails[0].FMSCOR_LetterHeadNo;

                    $scope.refnoflag = $scope.getreferncedetails[0].FMSCOR_ClientSupplierFlg;
                    $scope.refnoid = $scope.getreferncedetails[0].FMSCOR_Id;

                    if ($scope.refnoflag === "Internal") {
                        $scope.getviewemployeedetails = promise.getviewemployeedetails;
                    }
                }
            });
        };

        $scope.savethirdtabcorrespondence = function (obj4) {
            if ($scope.myForm31.$valid) {

                angular.forEach($scope.deptlist3, function (dd) {
                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.miid2 = dd.mI_Id;
                    }
                });

                var id2 = 0;
                if (obj4.FMSCORMD_Id > 0) {
                    id2 = obj4.FMSCORMD_Id;
                } else {
                    id2 = 0;
                }

                var data = {
                    "FMSCORMD_Id": id2,
                    "FMSCOR_Id": obj4.FMSCOR_Id.fmscoR_Id,
                    "FMSMDMD_Id": obj4.FMSMDMD_Id,
                    "FMSCORMD_SentDate": new Date(obj4.FMSCORMD_SentDate).toDateString(),
                    "FMSCORMD_ReferenceNo": obj4.FMSCORMD_ReferenceNo,
                    "FMS_Temp_UploadFiles_Mode": $scope.fmsmodeupload,
                    "MI_Id": $scope.miid2,
                    "HRMD_Id": $scope.HRMD_Id
                };

                apiService.create("FMS_Correspondence/savethirdtabcorrespondence", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.saveorupdate === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.saveorupdate === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update The Record");
                        }

                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                    $scope.OnChangeDept('Tab3');
                    //$scope.valuethirdtab = "Third";
                    //$scope.clear_third_tab($scope.valuethirdtab);
                });
            }
            else {
                $scope.submitted31 = true;
            }
        };

        $scope.Editdatathirdtab = function (obj4edit) {
            angular.forEach($scope.deptlist3, function (dd) {
                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                    $scope.miid2 = dd.mI_Id;
                }
            });

            $scope.flagtypes = [];
            if ($scope.objfiles.ClientSupplierFlgflag === "All") {
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Client" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Sales Lead" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Supplier" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Others" });
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: "Internal" });
            } else {
                $scope.flagtypes.push({ FMSCOR_ClientSupplierFlg: $scope.objfiles.ClientSupplierFlgflag });
            }

            var data = {
                "FMSCORMD_Id": obj4edit.fmscormD_Id,
                "MI_Id": $scope.miid2,
                "HRMD_Id": $scope.HRMD_Id,
                "flagtypes": $scope.flagtypes
            };

            apiService.create("FMS_Correspondence/Editdatathirdtab", data).then(function (promise) {
                if (promise !== null) {

                    $scope.editableflagmode = true;
                    $scope.getthirdeditdata = promise.getthirdeditdata;
                    $scope.getfmcoriddetails1 = promise.getfmcoriddetails1;

                    $scope.objmode.FMSCORMD_ReferenceNo = $scope.getthirdeditdata[0].fmscormD_ReferenceNo;
                    $scope.objmode.FMSCORMD_Id = $scope.getthirdeditdata[0].fmscormD_Id;
                    $scope.objmode.FMSMDMD_Id = $scope.getthirdeditdata[0].fmsmdmD_Id;
                    $scope.objmode.FMSCORMD_SentDate = new Date($scope.getthirdeditdata[0].fmscormD_SentDate);

                    $scope.fmsmodeupload = promise.getthirdeditdata;

                    angular.forEach($scope.fmsmodeupload, function (ddd) {
                        if (ddd.fmscormD_FilePath !== null && ddd.fmscormD_FilePath != "") {
                            var img = ddd.fmscormD_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            console.log("ddd.filetype : " + ddd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.fmscormD_FilePath;
                            }
                        }
                    });

                    $scope.getreferncedetails = promise.getreferncedetails;

                    $scope.details = $scope.getreferncedetails[0].Details;
                    $scope.refno = $scope.getreferncedetails[0].FMSCOR_RefernceNo;
                    $scope.refnodate = $scope.getreferncedetails[0].FMSCOR_Date;
                    $scope.refnosubject = $scope.getreferncedetails[0].FMSCOR_Subject;
                    $scope.refnodesc = $scope.getreferncedetails[0].FMSCOR_Description;
                    $scope.refnotowhom = $scope.getreferncedetails[0].FMSCOR_ToName;
                    $scope.refnoletterno = $scope.getreferncedetails[0].FMSCOR_LetterHeadNo;
                    $scope.refnoflag = $scope.getreferncedetails[0].FMSCOR_ClientSupplierFlg;
                    $scope.refnoid = $scope.getreferncedetails[0].FMSCOR_Id;


                    if ($scope.refnoflag === "Internal") {
                        $scope.getviewemployeedetails = promise.getviewemployeedetails;
                    }


                    //if (promise.getfmcoriddetails1[0].fmscoR_ClientSupplierFlg === "Client") {
                    //    $scope.geteditclientdata_third = promise.geteditclientdata_third;
                    //    if ($scope.geteditclientdata_third !== null && $scope.geteditclientdata_third.length > 0) {
                    //        $scope.objmode.ismmclT_Id = $scope.geteditclientdata_third[0];
                    //    } else {
                    //        swal("Client Details Not Found");
                    //    }
                    //} else if (promise.getfmcoriddetails1[0].fmscoR_ClientSupplierFlg === "Supplier") {
                    //    $scope.geteditsupplierdata_third = promise.geteditsupplierdata_third;
                    //    if ($scope.geteditsupplierdata_third !== null && $scope.geteditsupplierdata_third.length > 0) {
                    //        $scope.objmode.INVMS_Id = $scope.geteditsupplierdata_third[0];
                    //    } else {
                    //        swal("Supplier Details Not Found");
                    //    }
                    //} else if (promise.getfmcoriddetails1[0].fmscoR_ClientSupplierFlg === "Sales Lead") {
                    //    $scope.geteditsalesdata_third = promise.geteditsalesdata_third;
                    //    if ($scope.geteditsalesdata_third !== null && $scope.geteditsalesdata_third.length > 0) {
                    //        $scope.objmode.ISMSLE_Id = $scope.geteditsalesdata_third[0];
                    //    } else {
                    //        swal("Sales Lead Details Not Found");
                    //    }
                    //}

                    $scope.getreferncenolistmode = promise.getreferncenolist;

                    $scope.objmode.From_FMSCOR_Date = new Date(promise.getfmcoriddetails1[0].fmscoR_Date);
                    $scope.objmode.To_FMSCOR_Date = new Date(promise.getfmcoriddetails1[0].fmscoR_Date);
                    $scope.objmode.FMSMFC_Id = promise.getfmcoriddetails1[0].fmsmfC_Id;
                    angular.forEach($scope.getreferncenolistmode, function (dd) {
                        if (dd.fmscoR_Id === promise.getfmcoriddetails1[0].fmscoR_Id) {
                            $scope.objmode.FMSCOR_Id = dd;
                        }
                    });
                }
            });
        };

        $scope.activedeactivethirdtab = function (useer) {

            var data = {
                "FMSCORMD_Id": useer.fmscormD_Id
            };

            var mgs = "";
            var confirmmgs = "";
            if (useer.fmscormD_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

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
                        apiService.create("FMS_Correspondence/activedeactivethirdtab", data).then(function (promise) {
                            if (promise.messageval === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear_third_tab();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.clear_third_tab = function (data) {
            $scope.submitted3 = false;
            $scope.submitted31 = false;
            $scope.editableflagmode = false;
            $scope.objmode.ismmclT_Id = "";
            $scope.objmode.ISMMPR_Id = "";
            $scope.objmode.INVMS_Id = "";
            $scope.objmode.ISMSLE_Id = "";
            $scope.objmode.FMSCOR_Id = "";
            $scope.objmode.FMSMFC_Id = "";
            $scope.objmode.FMSMDMD_Id = "";
            $scope.objmode.FMSCOR_Id = "";
            $scope.objmode.FMSCORMD_ReferenceNo = "";

            $scope.getreferncedetails = [];
            $scope.fmsmodeupload = [];
            $scope.fmsmodeupload = [{ id: 'fmsmodecore1' }];           

            $scope.objmode.FMSCORMD_SentDate = null;
            $scope.objfiles.searchrefnotab3 = "";
            $scope.editableflagmode = false;
            $scope.fromdate3 = new Date($scope.today);
            $scope.todate3 = new Date($scope.today);
            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";
            $scope.onthirdtabcorrespondence(data);
        };

        $scope.clear_third_tab_load = function (data) {
            $scope.submitted3 = false;
            $scope.submitted31 = false;
            $scope.editableflagmode = false;
            $scope.objmode.ismmclT_Id = "";
            $scope.objmode.ISMMPR_Id = "";
            $scope.objmode.INVMS_Id = "";
            $scope.objmode.ISMSLE_Id = "";
            $scope.objmode.FMSCOR_Id = "";
            $scope.objmode.FMSMFC_Id = "";
            $scope.objmode.FMSMDMD_Id = "";
            $scope.objmode.FMSCORMD_ReferenceNo = "";

            $scope.getreferncedetails = [];
            $scope.fmsmodeupload = [];
            $scope.fmsmodeupload = [{ id: 'fmsmodecore1' }];

            //$scope.labeldetailsmode = "Client";
            //$scope.objmode.ClientSupplierFlgmode = "Client";
            if (dd !== 'Third') {
                $scope.getreferncenolistmode = [];
                $scope.objmode.From_FMSCOR_Date = null;
                $scope.objmode.To_FMSCOR_Date = null;
            }

            $scope.objmode.FMSCORMD_SentDate = null;
            $scope.objfiles.searchrefnotab3 = "";
            $scope.editableflagmode = false;
            $scope.fromdate3 = new Date($scope.today);
            $scope.todate3 = new Date($scope.today);
            $scope.details = "";
            $scope.refno = "";
            $scope.refnodate = "";
            $scope.refnosubject = "";
            $scope.refnodesc = "";
            $scope.refnotowhom = "";
            $scope.refnoletterno = "";
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.interacted31 = function (field) {
            return $scope.submitted31;
        };

        $scope.fmsmodeupload = [{ id: 'fmsmodecore1' }];

        $scope.uploadfmsfilesmodedocuments1 = [];

        $scope.uploadfmsdocmode = function (input, document) {
            $scope.uploadfmsfilesmodedocuments1 = input.files;
            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhotomode(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhotomode(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhotomode(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadfmsfilesmodedocuments1.length; i++) {
                formData.append("File", $scope.uploadfmsfilesmodedocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/fmsdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.fmscormD_FilePath = d;
                    data.fmscormD_FileName = $scope.filename;
                    $('#').attr('src', data.fmscormD_FilePath);
                    var img = data.fmscormD_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.fmscormD_FilePaths;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectimage = function (data, idd, typeofphoto, type) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: idd
                    })[0].click();
                });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.fmscorfL_SoftCopyFilePath;
            $scope.videdfd = data.fmscorfL_SoftCopyFilePath;
            $scope.movie = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.movie1 = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.fmscorfL_SoftCopyFilePath });
            console.log($scope.view_videos);
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
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
        };

        $scope.onclickdoc = function (path) {
            //  var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + data.hrmedS_DocumentImageName;
            $scope.detailFrame = $sce.trustAsResourceUrl(path);
            $('#myModaldocx').modal('show');
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();