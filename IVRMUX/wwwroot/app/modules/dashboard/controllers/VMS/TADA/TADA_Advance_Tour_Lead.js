(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_Advance_Tour_LeadController', TADA_Advance_TourController)
    TADA_Advance_TourController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', '$q', 'intracontroller', '$window']
    function TADA_Advance_TourController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $q, intracontroller, $window) {
        $scope.currentPage = 1;
        $scope.leaddetailsNew = [];
        $scope.leaddatas = [];
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.txt_Remarks = "";
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.CategoryFlag = "";
        $scope.Datacategorynew = [];
        $scope.getPlaner = []; $scope.VTADAA_BalanceConcal = 0;
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_Advance_Tour/get_ier_load", pageid).
                then(function (promise) {
                    if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {
                        $scope.IER_ID = "";
                        $scope.getPlaner = promise.tourPlanDetails;
                        $scope.IER_ID = promise.tourPlanDetails[0].IER_ID;
                        $scope.OnchangeRootlist();
                    }
                    else {
                        swal("TADA Not Applied !");
                    }
                   

                })
        }
        $scope.VTADAAA_StatusFlg = "";
        $scope.OnchangeRootlist = function () {
            $scope.employeeid = [];
            $scope.VTADAAA_StatusFlg = "";
            if ($scope.IER_ID > 0) {

                var data = {
                    "IER_ID": $scope.IER_ID,

                }
                apiService.create("TADA_Advance_Tour/Lead_Update", data).then(function (promise) {
                    if (promise.getloadarray != null && promise.getloadarray.length > 0) {
                        $scope.Datacategorynew = promise.getloadarray;
                        $scope.employeeid = [];
                        angular.forEach($scope.Datacategorynew, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            }
                            else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push(dev);
                                }
                            }
                        })
                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.Datacategorynew, function (dd) {
                                if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.Datacategorynew = $scope.templist;

                        });
                        $scope.vi = 1;

                        $scope.VTADAAA_StatusFlg = $scope.Datacategorynew[0].VTADAAA_StatusFlg;

                    }

                })
            }
            else {
                swal("Root Plan  Not Found !")
            }
            
        }
        //
        $scope.interactedtadalead = function (field) {
            return $scope.submitted1;
        };


        //Ganesh

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.ilrT_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto1(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto1(data) {
            console.log("TADA  Upload  :" + data);
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
                    data.ilrT_FilePath = d;
                    data.ilrT_FileName = $scope.ilrT_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };




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
        $scope.submitted = false;
        $scope.save_finalSubmission = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                $scope.CategoryPara = [];
                if ($scope.materaldocuuploadNew != null && $scope.materaldocuuploadNew.length > 0) {
                    angular.forEach($scope.materaldocuuploadNew, function (itm) {
                        if (itm.ilrT_FilePath !== undefined && itm.ilrT_FilePath !== null && itm.ilrT_FilePath !== "" && itm.ILRT_Cost > 0) {
                            $scope.uploadarray.push({
                                VTADAAF_FileName: itm.ilrT_FileName,
                                VTADAAF_FilePath: itm.ilrT_FilePath,
                                VTADAAF_Filedesc: itm.ilrT_Remarks,
                                VTADAAF_Cost: itm.ILRT_Cost,
                            });
                        }
                    });
                }
                //Ganesh

                if ($scope.CatParameters != null && $scope.CatParameters.length > 0) {
                    angular.forEach($scope.CatParameters, function (itm) {
                        angular.forEach(itm.DaTAPARA, function (itm1) {

                            if (itm1.ilrT_FilePath !== undefined && itm1.ilrT_FilePath !== null && itm1.ilrT_FilePath !== "") {
                                $scope.CategoryPara.push({
                                    ILRT_FileName: itm1.ilrT_FileName,
                                    ILRT_FilePath: itm1.ilrT_FilePath,
                                    ILRT_Remarks: itm1.ISMERCP_Remarks,
                                    //IERC_ID: itm1.IMRC_CategoryId,
                                    HRME_Id: itm1.IMRCP_Id,

                                });
                            }
                        });

                    });
                }

                //added
                var VTADAA_Id = 0; var VTADAA_BalanceConcal = 0;
                var VTADAAA_Id = 0;
                var totalamount = 0; var VTADAA_BalanceHRMS = 0;
                if ($scope.VTADAA_TotalAppliedAmount > 0) {
                    totalamount = $scope.VTADAA_TotalAppliedAmount;
                }
                if ($scope.VTADAA_BalanceHRMS > 0) {
                    VTADAA_BalanceHRMS = $scope.VTADAA_BalanceHRMS;
                }
                if ($scope.VTADAA_BalanceConcal > 0) {
                    VTADAA_BalanceConcal = $scope.VTADAA_BalanceConcal;
                }

                if ($scope.getPlaner != null && $scope.getPlaner.length > 0) {
                    angular.forEach($scope.getPlaner, function (emcp) {
                        if (emcp.IER_ID == $scope.IER_ID) {
                            VTADAAA_Id = emcp.VTADAAA_Id;
                            if (emcp.VTADAA_Id > 0) {
                                VTADAA_Id = emcp.VTADAA_Id;
                            }
                        }
                    });
                }
                var data = {
                    "VTADAA_ClientId": 0,
                    "VTADAA_TotalAppliedAmount": totalamount,
                    "VTADAA_Remarks": $scope.txt_Remarks,
                    "VTADAA_Id": VTADAA_Id,
                    "VTADAAA_Id": VTADAAA_Id,
                    filelistNew: $scope.uploadarray,
                    "VTADAA_BalanceHRMS": VTADAA_BalanceHRMS,
                    "IER_ID": $scope.IER_ID,
                    "VTADAA_BalanceConcal": VTADAA_BalanceConcal,
                    "filelist": $scope.CategoryPara
                };
                apiService.create("TADA_Advance_Tour/SaveFinalSubMission", data).then(function (promise) {
                    if (promise.returnvalue === true) {
                        if (promise.returnval === "Insert") {
                            swal("Record Saved Successfully", "", "success");
                        }
                        if (promise.returnval === "Failed") {
                            swal("Record Not saved", "", "error");
                        }
                        if (promise.returnval === "Duplicate") {
                            swal("Record Already Exist", "", "info");
                        }
                        if (promise.returnval === "Update") {
                            swal("Record Update Successfully", "", "success");
                            // $scope.onpageload();
                        }
                        if (promise.returnval === "UpdateFailed") {
                            swal("Record Not Update");
                        }
                        if (promise.returnval === "") {
                            swal("please contact administrator !", "", "error");
                            // $scope.onpageload();
                        }
                        if (promise.returnval === "RecordExit") {
                            swal(" U Already Adavance Applied  , Select Correct Between Dates !");
                            // $scope.onpageload();
                        }
                        if (promise.returnval === "DuplicateAdavance") {
                            swal(" U Already Adavance Applied  , Select Correct Between Dates Result Is Pending !");
                            //  $scope.onpageload();
                        }
                        $state.reload();
                    }
                    if (promise.returnvalue === false) {
                        if (promise.returnval === "") {
                            swal("Please contact Administrator !");
                            $state.reload();
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
          
        }
        $scope.save_tour_lead = function () {
            $scope.ILRR_Lat = '';
            $scope.ILRR_Long = '';
            $scope.submitted1 = true;
            if ($scope.myForm.$valid) {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        $timeout(function () {
                            $scope.ILRR_Lat = position.coords.latitude;
                            $scope.ILRR_Long = position.coords.longitude;
                            $scope.vmsatgpsL_LATLONGValue = d.toString() + "+" + dd.toString();

                        });
                    });
                }
                else {
                    swal("Please Enable Location...")
                }
                var values = {
                    "IER_ID": $scope.IERCLE_Id,
                    "ILRR_StatusRemark": $scope.ISMSLEDM_DemoExecuted,
                    "IER_Remarks": $scope.IER_Remarks,
                    "ILRR_Lat": $scope.ILRR_Lat,
                    "ILRR_Long": $scope.ILRR_Long,
                }
                apiService.create("TADA_Advance_Tour/TADA_Advance_Tour_Lead_save", values).then(function (response) {
                    if (response.message == "save") {
                        swal("Records Saved/Updated Sucessfully !", "", "success");
                    }
                    else if (response.message == "not save") {
                        swal("Failed To Saved !", "", "warning");
                    }
                    else if (response.message == "admin") {
                        swal("Please Contact Administrator !");
                    }
                    else {
                        swal("Records Not Saved  !");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted1 = false;
            }
        }
   $scope.OnChangeproduct = function (objr) {

            objr.bomList = [];
            objr.ISMSBOM_Id = "";
            var products = [];
            if (objr.ismsmpR_Id > 0) {
                products.push(objr.ismsmpR_Id);
                var data = {
                    "product": products,
                    "ISMSLE_Id": 0
                };
                apiService.create("Lead_Target_Mapping/OnChangeOnChangeproductlist", data).then(function (promise) {

                    if (promise.bomList != null && promise.bomList.length > 0) {
                        angular.forEach(promise.bomList, function (emp) {
                            objr.bomList.push({
                                ISMSBOM_Id: emp.ISMSBOM_Id,
                                ISMSBOM_BOMName: emp.ISMSBOM_BOMName
                            });

                        });


                    } else {
                        swal('Packages Are not Mapped ! ');

                    }

                });
            }
            else {

            }

        };
        //save_finalSubmission
        $scope.demo_response = function (bil) {           
            $scope.CategoryFlag = "";           
            var data = {
                "ISMSLEDM_Id": bil.ISMSLEDM_Id,
                "ISMSLE_Id": bil.ISMSLE_Id,
                "message":""
            }
            apiService.create("TADA_Advance_Tour/Edit_response_lead_demo", data).then(function (promise) {
                $scope.leaddetailsNew = [];
                $scope.demo_response_list = promise.demo_response_details;
                $scope.status_demo_master = promise.status_demo_master;
                $scope.CategoryFlag = bil.IMRC_CategoryName;//Closure
               // $scope.CategoryFlag = "Closure";
                $scope.StudentStrengthDemo = $scope.demo_response_list[0].ISMSLE_StudentStrength;

                $scope.leaddetailsNew = [$scope.demo_response_list[0]];

                $scope.leaddetailslength = $scope.leaddetailsNew.length;
            })
        }
        $scope.LeadsData = function (bil) {        
            $scope.LeadComments = [];
            var data = {
                "IER_ID": $scope.IER_ID,              
            }
            apiService.create("TADA_Advance_Tour/LeadComments", data).then(function (promise) {
                if (promise.view_lead_demo != null && promise.view_lead_demo.length > 0) {

                    $scope.LeadComments = promise.view_lead_demo;
                }
                else {
                    swal("Comments Not Found !")
                }

            })
        }
        //DemoResclick
        $scope.DemoResclick = function (evet) {
            if (evet.ISMSLECOM_SendQuotationFlg == true) {
                evet.ISMSLEDM_MOMFlg = false; evet.ISMSLEDM_NegotiationFlg = false; evet.ISMSLEDM_DemoCancelledFlg = false;
                evet.ISMSLEDM_NegotiationAmt = "";
            }
            if (evet.ISMSLEDM_MOMFlg == true) {
                evet.ISMSLECOM_SendQuotationFlg = false; evet.ISMSLEDM_NegotiationFlg = false; evet.ISMSLEDM_DemoCancelledFlg = false;
                evet.ISMSLEDM_NegotiationAmt = "";
            }
            if (evet.ISMSLEDM_NegotiationFlg == true) {
                evet.ISMSLECOM_SendQuotationFlg = false; evet.ISMSLEDM_MOMFlg = false; evet.ISMSLEDM_DemoCancelledFlg = false;
               
                evet.ISMSLEDM_NegotiationAmt = "";
                swal("Please Enter Negotiation Amount");
            }
            if (evet.ISMSLEDM_DemoCancelledFlg == true) {
                evet.ISMSLECOM_SendQuotationFlg = false; evet.ISMSLEDM_MOMFlg = false; evet.ISMSLEDM_NegotiationFlg = false;
                evet.ISMSLEDM_NegotiationAmt = "";
            }

        }

        //Edit_response_lead_demo
        $scope.all_checkC2 = function () {
            var checklist_temp = $scope.usercheckC2
            angular.forEach($scope.demo_response_list, function (item) {
                item.selectedd2 = checklist_temp
            });
        }
        //all_checkC2
        //Save_response
        $scope.get_evalistt = function () {

            $scope.usercheckC2 = $scope.demo_response_list.every(function (options) {
                return options.selectedd2;
            });
        }
        $scope.submitted1 = false;
        $scope.Save_response = function () {
            if ($scope.myForm1.$valid) {
                $scope.submitted1 = true;
                $scope.empchlist2 = [];
                $scope.leadverify = [];
                $scope.CategoryFlag1 = $scope.CategoryFlag;  
                let isValid = true;

                angular.forEach($scope.leaddetailsNew, function (emp) {
                    // Check if any field is null, empty, or undefined
                    if (!emp.ismslE_Id || !emp.ISMSLE_LeadName || !emp.ISMSLE_ContactPerson || !emp.ISMSLE_ContactNo ||
                        !emp.ISMSLE_LeadAddress1 || !emp.ISMSLE_LeadAddress2 || !emp.ISMSLE_LeadAddress3 ||
                        !emp.ISMSLE_EmailId || !emp.ISMSLE_ContactDesignation || !emp.ISMSLE_StudentStrength) {
                        isValid = false;
                        swal("Please Enter All the The Data...");
                        return;
                    }
                });


                if (isValid) {
                    angular.forEach($scope.leaddetailsNew, function (emp) {
                        $scope.leaddatas.push({
                            ISMSLE_Id: emp.ismslE_Id,
                            ISMSLE_LeadName: emp.ISMSLE_LeadName,
                            ISMSLE_ContactPerson: emp.ISMSLE_ContactPerson,
                            ISMSLE_ContactNo: emp.ISMSLE_ContactNo,
                            ISMSLE_LeadAddress1: emp.ISMSLE_LeadAddress1,
                            ISMSLE_LeadAddress2: emp.ISMSLE_LeadAddress2,
                            ISMSLE_LeadAddress3: emp.ISMSLE_LeadAddress3,
                            ISMSLE_EmailId: emp.ISMSLE_EmailId,
                            ISMSLE_ContactDesignation: emp.ISMSLE_ContactDesignation,
                            ISMSLE_StudentStrength: emp.ISMSLE_StudentStrength
                        });

                    });


                    angular.forEach($scope.demo_response_list, function (emp) {
                        var ismsmsT_StatusName = "";
                        angular.forEach($scope.status_demo_master, function (emcp) {
                            if (emcp.ismsmsT_Id == emp.ISMSMST_Id && emp.selectedd2 === true) {
                                ismsmsT_StatusName = emcp.ismsmsT_StatusName;
                            }
                        });




                        if (emp.selectedd2 === true) {
                            var NON_Amount = 0;
                            if (emp.ISMSLEDM_NegotiationAmt > 0) {
                                NON_Amount = emp.ISMSLEDM_NegotiationAmt;
                            }

                            if ($scope.CategoryFlag1 != "Closure") {
                                if (emp.ISMSMST_Id == null || emp.ISMSMST_Id == undefined || emp.ISMSMST_Id == "") {
                                    swal("Please select the Status...");
                                    return;

                                }
                                if (emp.ISMSBOM_Id == null || emp.ISMSBOM_Id == undefined || emp.ISMSBOM_Id == "") {
                                    swal("Please select the Package...");
                                    return;
                                }
                            }
                            


                            var ISMSMST_Id = 0;
                            if (emp.ISMSMST_Id > 0) {

                                ISMSMST_Id = emp.ISMSMST_Id;
                            }

                            $scope.empchlist2.push({
                                ISMSLEDM_Id: emp.ismsledM_Id,
                                ISMSMPR_Id: emp.ismsmpR_Id,
                                ISMSMST_Id: ISMSMST_Id,
                                ISMSLEDMPR_Remarks: emp.ISMSLEDMPR_Remarks,
                                ISMSLECOM_SendQuotationFlg: emp.ISMSLECOM_SendQuotationFlg,
                                ISMSLECOM_MoMFlg: emp.ISMSLEDM_MOMFlg,
                                ISMSLECOM_NonFlg: emp.ISMSLEDM_NegotiationFlg,
                                NON_Amount: NON_Amount,
                                ISMSLECOM_CancelFlg: emp.ISMSLEDM_DemoCancelledFlg,
                                SMSLEDM_CategaroyId: emp.SMSLEDM_CategaroyId,
                                IER_ID: $scope.IER_ID,
                                ISMSLEDMPR_ActiveFlag: false,
                                ISMSMST_StatusName: ismsmsT_StatusName,
                                ISMSLE_Id: emp.ismslE_Id,
                                ISMSMPR_ProductName: emp.ismsmpR_ProductName,
                                ISMSBOM_Id: emp.ISMSBOM_Id

                            });

                            $scope.ILRR_Lat = ""; $scope.ILRR_Long = ""; $scope.vmsatgpsL_LATLONGValue = "";
                            if (navigator.geolocation) {
                                navigator.geolocation.getCurrentPosition(function (position) {
                                    $timeout(function () {
                                        $scope.ILRR_Lat = position.coords.latitude;
                                        $scope.ILRR_Long = position.coords.longitude;
                                        //$scope.vmsatgpsL_LATLONGValue = d.toString() + "+" + dd.toString();

                                    });
                                });
                            }

                            var data = {
                                product_demo_master_temp2: $scope.empchlist2,
                                "ILRR_Long": $scope.ILRR_Long,
                                "ILRR_Lat": $scope.ILRR_Lat,
                                // "ISMSLE_STUDENTSTRENGTH": $scope.StudentStrengthDemo,
                                "leaddetails": $scope.leaddatas

                            }
                            //Save_response_lead_demo
                            apiService.create("TADA_Advance_Tour/Save_response_lead_demo", data).then(function (promise) {
                                if (promise.return_status === "Update") {
                                    swal('Update Successfully')
                                    $('#myModalgetdemoresponselist').modal('hide');
                                    $state.reload();
                                    //$scope.save_tour_lead();
                                }
                                else {
                                    swal('Opeartion Failed !')
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal('Please select The checkBox To Proceed..');
                        }
                    });
                }
            }
            else {
                $scope.submitted1 = true;
            }
        }

        $scope.Cancelnew = function () {
            $('#myModalgetdemoresponselist').modal('hide');
            $scope.onpageload
        }
        $scope.view = function (ss, vi) {
            if (vi == 1) {
                $scope.viewall = 1;
            }
            else {
                $scope.viewall = 0;
            }
            //$scope.edibl = ss.ismsledM_Id;
            //$scope.edib2 = ss.ismslE_Id;
            var data = {
                "viewall": $scope.viewall,
                "ISMSLEDM_Id": ss.ISMSLEDM_Id,
                "ISMSLE_Id": ss.ISMSLE_Id
            }
            apiService.create("TADA_Advance_Tour/view_lead_demo", data).then(function (promise) {
                $scope.ismslE_LeadName_s = promise.view_lead_demo[0].ismslE_LeadName;
                $scope.ISMSLEDM_ContactPerson_s = promise.view_lead_demo[0].ismsledM_ContactPerson;
                $scope.ISMSLEDM_DemoAddress_s = promise.view_lead_demo[0].ismsledM_DemoAddress;
                $scope.ISMSLEDM_Remarks_s = promise.view_lead_demo[0].ismsledM_Remarks;
                $scope.ISMSLE_ContactNo_s = promise.view_lead_demo[0].ismslE_ContactNo;

                $scope.product_dd_s = promise.product_dd_s;
                if ($scope.viewall == 1) {
                    $scope.demo_response_details = promise.demo_response_details;
                }


            });
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.uploadtadaroutedoc = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.ilrT_FileName = input.files[0].name;
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
            console.log("TADA  Upload  :" + data);
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
                    data.ilrT_FilePath = d;
                    data.ilrT_FileName = $scope.ilrT_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        $scope.materaldocuuploadNew = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuuploadNew.length === 1) {
            $scope.cnt = 1;
        }
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                    $scope.materaldocuupload[i].editflag = false;
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
            $scope.ilrT_FilePath = img;
            var img = $scope.ilrT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#tadapreviewX').attr('src', $scope.ilrT_FilePath);
                    $('#myimagePreviewtada').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ilrT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ilrT_FilePath;
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
                $window.open($scope.ilrT_FilePath)
            }
        };

        //

        $scope.save_tada_doc_dta = function () {

            $scope.uploadarray = [];
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (itm) {
                    if (itm.ilrT_FilePath !== undefined && itm.ilrT_FilePath !== null && itm.ilrT_FilePath !== "" && itm.ILRT_Cost > 0 && itm.editflag != true) {
                        $scope.uploadarray.push({
                            ILRT_FileName: itm.ilrT_FileName,
                            ILRT_FilePath: itm.ilrT_FilePath,
                            ILRT_Remarks: itm.ilrT_Remarks,
                            ILRT_Cost: itm.ILRT_Cost,

                        });
                    }
                });
            }

            if ($scope.uploadarray != null && $scope.uploadarray.length > 0) {

            }
            else {
                swal("Update Atleaset One File !!");
                return;
            }

            var data = {
                "IER_ID": $scope.IER_ID,
                "IERC_ID": $scope.IERC_ID,
                filelist: $scope.uploadarray,
            };
            apiService.create("TADA_Advance_Tour/savedta", data).then(function (promise) {
                if (promise.message === "Saved") {
                    swal("Record Saved Successfully", "", "success");
                    $state.reload();
                }
                if (promise.message === "FailedSave") {
                    swal("Record Not saved", "", "cancel");
                    $state.reload();
                }
                if (promise.message === " ") {
                    swal("please contact administrator !");
                    $state.reload();
                }
            });

        }

        //Cancel
        $scope.cancel_tada = function () {
            $state.reload();
        };
        $scope.save_Finaltour = function () {
            //TADA_Advance_Tour
            $scope.VTADAA_TotalAppliedAmount = 0; $scope.VTADAA_BalanceHRMS = 0;
            $scope.leadLists = [];
            $scope.getArrayDc = []; $scope.EditArrayTwo = []; $scope.TourPlanDetails = []; $scope.FinalSumbmission = []
            $scope.ApprovalSubmission = []; $scope.CategoryParameters = [];
            if ($scope.Datacategorynew != null && $scope.Datacategorynew.length > 0) {
                var data = {
                    "IER_ID": $scope.Datacategorynew[0].IER_ID,
                };
                apiService.create("TADA_Advance_Tour/FinalSubMission", data).then(function (promise) {
                    var ILRT_Cost = 0;
                    if (promise.getArray != null && promise.getArray.length > 0) {
                        $scope.getArrayDc = promise.getArray;
                    }
                    if (promise.editArrayTwo != null && promise.editArrayTwo.length > 0) {
                        $scope.EditArrayTwo = promise.editArrayTwo;
                    }
                    if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {
                        $scope.TourPlanDetails = promise.tourPlanDetails;
                    }
                    //ConcalLopdata
                    $scope.concalLopdata = []; $scope.VTADAA_BalanceConcal = "";
                    if (promise.concalLopdata != null && promise.concalLopdata.length > 0) {
                        $scope.concalLopdata = promise.concalLopdata;
                        $scope.VTADAA_BalanceConcal = (Number($scope.concalLopdata[0].TotalSactionedAmount) - (Number($scope.concalLopdata[0].TotalEligbleAmount)));
                    }
                    //getloadarray
                    if (promise.getloadarray != null && promise.getloadarray.length > 0) {     
                                              
                        $scope.materaldocuuploadNew = promise.getloadarray;
                        angular.forEach($scope.materaldocuuploadNew, function (dd) {
                            ILRT_Cost = (Number(ILRT_Cost) + (Number(dd.ILRT_Cost)));
                            dd.ilrT_FileName = dd.ILRT_FileName;
                            dd.ilrT_FilePath = dd.ILRT_FilePath;
                            dd.ilrT_Remarks = dd.ILRT_Remarks;
                            dd.ilrT_Cost = dd.ILRT_Cost;
                            dd.editflag = true;
                        });
                    }
                    //LeadLists
                    if (promise.leadLists != null && promise.leadLists.length > 0) {
                        $scope.leadLists = promise.leadLists;
                        $scope.employeeidNew = [];

                        angular.forEach($scope.leadLists, function (dev) {
                            if ($scope.employeeidNew.length === 0) {
                                $scope.employeeidNew.push(dev);
                            }
                            else if ($scope.employeeidNew.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeidNew, function (emp) {
                                    if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeidNew.push(dev);
                                }
                            }
                        })
                        angular.forEach($scope.employeeidNew, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.leadLists, function (dd) {
                                if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.Datacategorynew = $scope.templist;

                        });
                    }
                    //FinalSumbmission //ApprovalSubmission
                    if (promise.finalSumbmission != null && promise.finalSumbmission.length > 0) {
                        $scope.FinalSumbmission = promise.finalSumbmission;
                    }

                    //Category parameters

                    if (promise.editdataOthelist != null && promise.editdataOthelist.length > 0) {
                        $scope.CategoryParameters = promise.editdataOthelist;
                        $scope.CatParameters = [];
                        angular.forEach($scope.CategoryParameters, function (dev) {
                            if ($scope.CatParameters.length === 0) {
                                $scope.CatParameters.push(dev);
                            }
                            else if ($scope.CatParameters.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.CatParameters, function (emp) {
                                    if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.CatParameters.push(dev);
                                }
                            }
                        })
                        angular.forEach($scope.CatParameters, function (ddd) {
                            $scope.tempPARA = [];
                            angular.forEach($scope.CategoryParameters, function (dd) {
                                if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                    $scope.tempPARA.push(dd);
                                }
                            });
                            ddd.DaTAPARA = $scope.tempPARA;

                        });



                    }

                    if (promise.approvalSubmission != null && promise.approvalSubmission.length > 0) {
                        $scope.ApprovalSubmission = promise.approvalSubmission;
                       // $scope.VTADAA_TotalAppliedAmount = promise.approvalSubmission[0].EligibleAmtToApply;

                        $scope.VTADAA_BalanceHRMS = promise.approvalSubmission[0].SalaryCreditAmt;
                        if ($scope.VTADAA_BalanceHRMS > 0) {

                        }
                        else {
                            $scope.VTADAA_TotalAppliedAmount = (Number(promise.approvalSubmission[0].TotalAmt) - (Number(promise.approvalSubmission[0].SactionedAmt))).toFixed(2);
                        }
                      
                       
                    }

                    var counttotal1 = 0;
                    var counttotal2 = 0;
                    var counttotal3 = 0;
                    $scope.TotalAppliedAmount = ""; $scope.TotalSactionedAmount = ""; $scope.TotalEligbleAmount = "";
                    $scope.ILRT_BillCost = "";
                    if ($scope.FinalSumbmission != null && $scope.FinalSumbmission.length > 0) {
                        angular.forEach($scope.FinalSumbmission, function (d) {

                            counttotal1 += d.VTADAAA_TotalAppliedAmount;
                            counttotal2 += d.VTADAAA_TotalSactionedAmount;
                            counttotal3 += d.VTADAAA_TotalEligbleAmount;

                        });
                        $scope.TotalAppliedAmount = Math.round(Number(counttotal1));
                        $scope.TotalSactionedAmount = Math.round(Number(counttotal2));
                        $scope.TotalEligbleAmount = Math.round(Number(counttotal3));
                        //Math.round(Number(Totalstudent)); //Math.round(Number(Totalstudent))

                    }

                   
                       $scope.ILRT_BillCost = ILRT_Cost;
                   

                });
            }

        }
        $scope.addgrnrowsNew = function () {
            if ($scope.materaldocuuploadNew != null && $scope.materaldocuuploadNew.length > 1) {
                for (var i = 0; i === $scope.materaldocuuploadNew.length; i++) {
                    var id = $scope.materaldocuuploadNew[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                    $scope.materaldocuuploadNew[i].editflag = false;
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuuploadNew.push({ 'itrS_Id': 'trans' + newItemNo });
        };
        $scope.removegrnrowsNew = function (index, data) {
            var newItemNo = $scope.materaldocuuploadNew.length - 1;
            $scope.materaldocuuploadNew.splice(index, 1);

        };

        //SaveFinalSubMission
        //RemoveData
        $scope.RemoveData = function (emplist) {
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
            $scope.IER_ID = ""; $scope.IERC_ID = "";
            $scope.IER_ID = emplist.IER_ID;
            $scope.IERC_ID = emplist.IERC_ID;
            var data = {
                "IER_ID": $scope.IER_ID,
                "IERC_ID": $scope.IERC_ID,

            };
            apiService.create("TADA_Advance_Tour/Tadadata", data).then(function (promise) {
               
                if (promise.getloadarray != null && promise.getloadarray.length > 0) {
                    $scope.materaldocuupload = promise.getloadarray;

                    angular.forEach($scope.materaldocuupload, function (dd) {
                      
                        dd.ilrT_FileName = dd.ILRT_FileName;
                        dd.ilrT_FilePath = dd.ILRT_FilePath;
                        dd.ilrT_Remarks = dd.ILRT_Remarks;
                        dd.ilrT_Cost = dd.ILRT_Cost;
                        dd.editflag = true;
                    });

                }
                else {
                    angular.forEach($scope.materaldocuupload, function (dd) {

                        dd.editflag = false;
                    });
                }

            });

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;;
        };


        $scope.isOptionsRequired = function () {
            return !$scope.status_demo_master.some(function (options) {
                return options.selected;
            });
        };


        $scope.isOptionsRequired1 = function () {
            return !$scope.objr.bomList.some(function (options) {
                return options.selected;
            });
        };
    }
})();

