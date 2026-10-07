(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Sales_Lead_DemoController', iSM_Sales_Lead_DemoController)

    iSM_Sales_Lead_DemoController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function iSM_Sales_Lead_DemoController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.ISMSLECOM_SendQuotationFlg = false;

        $scope.EmployeeDemoRatingList = [];
        $scope.isEditChecked = false; 
        $scope.isVerified = false;
        $scope.sales_swappingEmpData = [];
        $scope.objques = {};
        $scope.Cancel_Remarks = '';
        $scope.sortKey = 'ISMSLEDMPR_Id';
        $scope.sortReverse = true;
        //  $scope.ISMSLEDM_DemoScheduledBy = 1860;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.objr = {};
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //============= + - row===============
        $scope.demonstration = [{ itrS_Id: 'trans1' }];
        if ($scope.demonstration.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addpirows = function () {
            if ($scope.demonstration.length > 1) {
                for (var i = 0; i === $scope.demonstration.length; i++) {
                    var id = $scope.demonstration[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.demonstration.push({ 'itrS_Id': 'trans' + newItemNo });

        };
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.demonstration.length - 1;
            $scope.demonstration.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };
        $scope.RoomList = function () {
            $scope.hrmR_RoomName = "";
            angular.forEach($scope.hrmr_list_dd, function (tt) {
                if (tt.hrmR_Id == $scope.HRMR_Id) {
                    $scope.hrmR_RoomName = tt.hrmR_RoomName;
                }
            })
        }
        $scope.lead_list_dd = [];
        //get data
        $scope.LoadData = function () {
            $scope.sales_swappingEmpData = [];
            $scope.currentPage = 1;
            //  $scope.lead_list_dd = [];
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Sales_Lead/get_load_lead_demo", pageid)
                .then(function (promise) {
                    $scope.lead_list_dd = promise.lead_list_dd;
                    $scope.hrme_list_dd = promise.hrme_list_dd;
                    $scope.hrme_list_ddd = promise.hrme_list_ddd;
                    $scope.hrmr_list_dd = promise.hrmr_list_dd;
                  //  $scope.product_list_dd = promise.product_list_dd;
                    $scope.demo_list = promise.demo_list;
                    $scope.presentCountgrid = $scope.demo_list.length;

                    $scope.demo_listnew = promise.demo_listnew;
                    //$scope.presentCountgrid = $scope.demo_listnew.length;
                   // $scope.sales_swappingEmpData = promise.sales_swappingEmpData;
                    if (promise.incentivelead != null && promise.incentivelead.length > 0) {
                        $scope.EmployeeDemoRatingList = promise.incentiveleads;
                    }

                   
                    $scope.vi = 1;

                })
        }
        //hrme_list_ddd
        $scope.Demo_Scheduled = function () {
            $scope.hrmdeS_DesignationName = ""; $scope.employeename = "";
            angular.forEach($scope.hrme_list_ddd, function (tt) {
                if ($scope.ISMSLEDM_DemoScheduledBy == tt.hrmE_Id) {
                    $scope.hrmdeS_DesignationName = tt.HRMDES_DesignationName;
                    $scope.employeename = tt.hrmE_EmployeeFirstName;
                    return;
                }
            })
        }

        $scope.get_ContactPerson = function () {
            $scope.ismslE_LeadName = ""; $scope.addres = ""; $scope.ismslE_EmailId = ""; $scope.ismslE_ContactNo = "";
            $scope.ISMSLE_ContactPerson = ""; $scope.ismslE_ContactDesignation = ""; $scope.ismslE_ContactDesignation = "";
            $scope.ISMSLE_Reference = "";
            angular.forEach($scope.lead_list_dd, function (tt) {
                if ($scope.ISMSLE_Id.ISMSLE_Id == tt.ISMSLE_Id) {
                    $scope.ISMSLE_ContactPerson = tt.ISMSLE_ContactPerson;
                    $scope.ismslE_ContactDesignation = tt.ismslE_ContactDesignation;
                    $scope.ismslE_LeadName = tt.ISMSLE_LeadNamenew;
                    $scope.addres = tt.ISMSLE_LeadAddress1;
                    $scope.ismslE_EmailId = tt.ismslE_EmailId;
                    $scope.ismslE_ContactNo == tt.ismslE_ContactNo;
                    $scope.ISMSLE_Reference = tt.ISMSLE_Reference;
                }
            })
            //onreport
            $scope.product_list_dd = [];            
            var data = {
                "reporttype": "Product",
                "ISMSLE_Id": $scope.ISMSLE_Id.ISMSLE_Id 
            }

            apiService.create("Sales_Lead/Edit_details_lead_demo", data).
                then(function (promise) {
                    if (promise.product_list_dd != null && promise.product_list_dd.length > 0) {
                        $scope.product_list_dd = promise.product_list_dd;
                    }
                    
                })

        };



        // edit
        $scope.edibl = {};
        $scope.edibl2 = {};
        $scope.edibl = 0;
        $scope.edib2 = 0;
        $scope.edit = function (bil) {
            $scope.product_list_dd = [];
            $scope.edibl = bil.ismsledM_Id;
            $scope.edib2 = bil.ismslE_Id;
            var data = {
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }

            apiService.create("Sales_Lead/Edit_details_lead_demo", data).
                then(function (promise) {


                    $scope.ISMSLE_LeadName = promise.edit_details_lead_demo[0].ismslE_LeadName;
                    $scope.ISMSLEDM_DemoType = promise.edit_details_lead_demo[0].ismsledM_DemoType;
                    $scope.ISMSLE_ContactPerson = promise.edit_details_lead_demo[0].ismsledM_ContactPerson;
                    $scope.HRME_Id = promise.edit_details_lead_demo[0].hrmE_Id;
                    $scope.employeename = promise.edit_details_lead_demo[0].employeename;
                    $scope.ISMSLEDM_DemoDate = new Date(promise.edit_details_lead_demo[0].ismsledM_DemoDate);
                    $scope.ISMSLEDM_DemoAddress = promise.edit_details_lead_demo[0].ismsledM_DemoAddress;
                    $scope.ISMSLEDM_Remarks = promise.edit_details_lead_demo[0].ismsledM_Remarks;
                    $scope.id = promise.edit_details_lead_demo[0].ismsledM_Id;
                    $scope.HRMR_Id = promise.edit_details_lead_demo[0].HRMR_Id;
                    $scope.ISMSLEDM_DemoScheduledBy = promise.edit_details_lead_demo[0].ISMSLEDM_DemoScheduledBy;
                    // $scope.ISMSLE_ContactPerson = promise.edit_details_lead_demo[0].ISMSLE_ContactPerson;
                    $scope.differentarray = [];
                    $scope.demonstration = [];
                    $scope.leadstration = [];
                    $scope.differentarray = promise.lead_Demo_Products_list;

                    if ($scope.differentarray.length > 0) {
                        angular.forEach($scope.differentarray, function (tt) {
                            $scope.demonstration.push({
                                ismsmpR_ProductName: tt.ismsmpR_ProductName,
                                ismsmpR_Id: tt.ismsmpR_Id,
                                ISMSLEDMPR_DiscussionPoints: tt.ismsledmpR_DiscussionPoints,

                            });
                        })
                    }
                    //edit view 
                    $scope.ismslE_LeadName = ""; $scope.addres = ""; $scope.ismslE_EmailId = ""; $scope.ismslE_ContactNo = "";
                    $scope.ISMSLE_ContactPerson = ""; $scope.ismslE_ContactDesignation = ""; $scope.ismslE_ContactDesignation = "";
                    $scope.ISMSLE_Reference = "";
                    angular.forEach($scope.lead_list_dd, function (tt) {
                        if (bil.ismslE_Id == tt.ISMSLE_Id) {
                            $scope.ISMSLE_ContactPerson = tt.ISMSLE_ContactPerson;
                            $scope.ismslE_ContactDesignation = tt.ismslE_ContactDesignation;
                            $scope.ismslE_LeadName = tt.ISMSLE_LeadNamenew;
                            $scope.addres = tt.ISMSLE_LeadAddress1;
                            $scope.ismslE_EmailId = tt.ismslE_EmailId;
                            $scope.ismslE_ContactNo = tt.ismslE_ContactNo;
                            $scope.ISMSLE_Reference = tt.ISMSLE_Reference;
                        }
                    })
                    //product_list_dd
                    if (promise.product_list_dd != null && promise.product_list_dd.length > 0) {
                        $scope.product_list_dd = promise.product_list_dd;
                    }
                   
                })
        };
        //get demo response
        $scope.demo_response = function (bil) {
            $scope.demo_listnew = [];
            $scope.edibl = bil.ismsledM_Id;
            $scope.edib2 = bil.ismslE_Id;
            var data = {
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }
            apiService.create("Sales_Lead/Edit_response_lead_demo", data).then(function (promise) {
                $scope.demo_response_list = promise.demo_response_list;
                $scope.status_demo_master = promise.status_demo_master;
                $scope.demo_listnew = promise.demo_listnew;
                $scope.StudentStrengthDemo = $scope.demo_response_list[0].ismslE_STUDENTSTRENGTH;

            })
        }
        $scope.all_checkC2 = function () {
            var checklist_temp = $scope.usercheckC2
            angular.forEach($scope.demo_response_list, function (item) {
                item.selectedd2 = checklist_temp
            });
        }
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {

            $scope.usercheckC2 = $scope.demo_response_list.every(function (options) {
                return options.selectedd2;
            });
        }

        $scope.updateCheckbox2 = function (evet) {
            if (evet.ISMSLECOM_SendQuotationFlg == true) {
                return evet.ISMSLECOM_SendQuotationFlg;
            }
        };


        //update demo response
        $scope.submitted1 = false;
        $scope.Save_response = function () {
            if ($scope.myForm1.$valid) {
                $scope.submitted1 = true;
                $scope.empchlist2 = [];
                $scope.leadverify = [];
                $scope.studentstrength = $scope.StudentStrengthDemo;
                angular.forEach($scope.demo_response_list, function (emp) {
                    var ismsmsT_StatusName = "";
                    angular.forEach($scope.status_demo_master, function (emcp) {
                        if (emcp.ismsmsT_Id == emp.ISMSMST_Id) {
                            ismsmsT_StatusName = emcp.ismsmsT_StatusName;
                        }
                    });

                    if (emp.selectedd2 === true) {
                        var ISMSBOM_Id = 0;
                        if (emp.ISMSBOM_Id > 0) {
                            ISMSBOM_Id = emp.ISMSBOM_Id;
                        }
                        $scope.empchlist2.push({
                            ISMSLEDM_Id: emp.ismsledM_Id, ISMSMPR_Id: emp.ismsmpR_Id,
                            ISMSMST_Id: emp.ISMSMST_Id, ISMSLEDMPR_Remarks: emp.ISMSLEDMPR_Remarks,
                            ISMSLECOM_SendQuotationFlg: emp.ISMSLECOM_SendQuotationFlg,
                            ISMSBOM_Id: ISMSBOM_Id, ISMSMPR_ProductName: emp.ismsmpR_ProductName,
                            ISMSLE_Id: emp.ismslE_Id
                        });
                    }

                });


                if ($scope.empchlist2 != null && $scope.empchlist2.length > 0) {
                    var data = {
                        product_demo_master_temp2: $scope.empchlist2,
                        "ISMSLE_STUDENTSTRENGTH": $scope.studentstrength

                    }
                    apiService.create("Sales_Lead/Save_response_lead_demo", data).then(function (promise) {
                        if (promise.return_status === "Update") {
                            swal('Update Successfully')
                            $state.reload();
                        }
                        else {
                            swal('Opeartion Failed !')
                            $state.reload();
                        }
                    });
                }
                else {
                    swal('Please select checkbox to Update Demo Feedback')

                }
            } else {
                $scope.submitted1 = true;
            }

        }

        //save
        $scope.submitted = false;
        $scope.submittednew = false;
        //Savedata_td
        $scope.Savedata_Modal = function () {
            $scope.submitted = true;
            $scope.submittednew = true;
            if ($scope.myFormdemo.$valid) {
                //if ($scope.dataform1.$valid) 
                {
                    $('#mailModal').modal('show');
            }
            //else {
            //        $scope.submitted = true;
            //    }

            } else {
                $scope.submittednew = true;
            }
        }
        $scope.Savedata_td = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $('#mailModal').modal('hide');
                $scope.product_list_tmp = [];
                angular.forEach($scope.demonstration, function (lia) {
                    $scope.product_list_tmp.push({
                        ISMSMPR_Id: lia.ismsmpR_Id,
                        ISMSLEDMPR_DiscussionPoints: lia.ISMSLEDMPR_DiscussionPoints
                    });
                });
                var ISMSLE_Id = 0;
                if ($scope.edib2 > 0) {
                    ISMSLE_Id = $scope.edib2;
                }
                else {
                    ISMSLE_Id = $scope.ISMSLE_Id.ISMSLE_Id;
                }
                var msg = document.getElementById("ordertemplate").innerHTML;;
                var data = {
                    "ISMSLEDM_Id": $scope.id,
                    "HRMR_Id": $scope.HRMR_Id,
                    "ISMSLEDM_DemoScheduledBy": $scope.ISMSLEDM_DemoScheduledBy,
                    "ISMSLE_Id": ISMSLE_Id,
                    "ISMSLEDM_DemoType": $scope.ISMSLEDM_DemoType,
                    "ISMSLEDM_ContactPerson": $scope.ISMSLE_ContactPerson,
                    "HRME_Id": $scope.HRME_Id,
                    "ISMSLEDM_DemoDate": new Date($scope.ISMSLEDM_DemoDate).toDateString(),
                    "ISMSLEDM_DemoAddress": $scope.ISMSLEDM_DemoAddress,
                    "ISMSLEDM_Remarks": $scope.ISMSLEDM_Remarks,
                    product_demo_master_temp1: $scope.product_list_tmp,
                    "ISMSLE_LEADADDRESS1": msg
                }
                apiService.create("Sales_Lead/SaveEdit_lead_demo", data).
                    then(function (promise) {

                        if (promise.return_status === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.return_status === "Add") {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.return_status === "Duplicate") {
                            swal('Record Already Exists,Please schedule To Other Employee');
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        $scope.Clearidd = function () {
            $state.reload();
        };
        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.cancel = function () {
            $('#CancelReasonModel').modal('hide');
            $scope.Cancel_Remarks = '';
            $scope.LoadData();
        }

        //Views
        $scope.view = function (ss, vi) {

            if (vi == 1) {
                $scope.viewall = 1;
            }
            else {
                $scope.viewall = 0;
            }

            $scope.edibl = ss.ismsledM_Id;
            $scope.edib2 = ss.ismslE_Id;
            var data = {
                "viewall": $scope.viewall,
                "ISMSLEDM_Id": $scope.edibl,
                "ISMSLE_Id": $scope.edib2
            }
            apiService.create("Sales_Lead/view_lead_demo", data).then(function (promise) {

                $scope.ISMSLEDM_DemoAddress_s = promise.view_lead_demo[0].ISMSLEDM_DemoAddress;
                $scope.ISMSLEDM_Remarks_s = promise.view_lead_demo[0].ISMSLEDM_Remarks
                $scope.ismslE_LeadName_s = promise.view_lead_demo[0].ISMSLE_LeadName
                $scope.ISMSLEDM_ContactPerson_s = promise.view_lead_demo[0].ISMSLEDM_ContactPerson
                $scope.ISMSLE_NoOfInstitutions_s = promise.view_lead_demo[0].ISMSLE_NoOfInstitutions
                $scope.ismsmcA_CategoryName_s = promise.view_lead_demo[0].ISMSMCA_CategoryName
                $scope.ISMSLE_ContactNo_s = promise.view_lead_demo[0].ISMSLE_ContactNo
                $scope.ISMSLE_StudentStrength_s = promise.view_lead_demo[0].ISMSLE_StudentStrength
                $scope.ismsmsT_StatusName_s = promise.view_lead_demo[0].ISMSMST_StatusName
                $scope.ISMSLE_EmailId_s = promise.view_lead_demo[0].ISMSLE_EmailId
                $scope.ISMSLE_StaffStrength_s = promise.view_lead_demo[0].ISMSLE_StaffStrength
                $scope.ismsmsO_SourceName_s = promise.view_lead_demo[0].ISMSMSO_SourceName
                $scope.ISMSLE_LeadAddress_s = promise.view_lead_demo[0].ISMSLE_LeadAddress1
                $scope.ISMSLE_Remarks_s = promise.view_lead_demo[0].ISMSLE_Remarks
                $scope.ISMSLE_Reference_s = promise.view_lead_demo[0].ISMSLE_Reference
                $scope.ivrmmS_Name_s = promise.view_lead_demo[0].IVRMMS_Name
                $scope.ivrmmC_CountryName_s = promise.view_lead_demo[0].IVRMMC_CountryName
                $scope.ISMSLE_ContactPerson=promise.view_lead_demo[0].ISMSLE_ContactPerson

                $scope.product_dd_s = promise.product_dd_s;
                if ($scope.viewall == 1) {
                    $scope.demo_response_details = promise.demo_response_details;
                }


            });
        }


        //deactive
        $scope.deactive = function (bLL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismsmpR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Product?",
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
                        apiService.create("Sales_Lead/deactivate_pro", bLL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Master Product Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Master Product Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };



        $scope.Reschedule = function () {
            $scope.Reschedule = [];
            angular.forEach($scope.demo_list, function (itm1) {
                if (itm1.ISMSLEDM_RescheduleFlag == true) {
                    $scope.Reschedule.push({ ismsledM_Id: itm1.ismsledM_Id })
                }
            });

            if ($scope.Reschedule != null && $scope.Reschedule.length > 0) {
                swal({
                    title: "Are You Sure To Reschedule",
                    text: "Do You Want To update..?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ISMSLEDM_Id": $scope.Reschedule,
                            }
                            apiService.create("Sales_Lead/Reschedule", data).
                                then(function (promise) {
                                    if (promise.msg == 'Reschedule') {
                                        swal("Reschedule Successfully!");
                                        $state.reload();
                                    }
                                    else if (promise.msg == "Not-Reschedule") {
                                        swal(" Please Select Verify Check box");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Sorry...something went wrong");
                                        $state.reload();
                                    }

                                })

                        }

                    });

            } else {
                $state.reload();
            }
        };

        $scope.swap = function (fll) {
            if (fll.selectedSwap) {

                $scope.fll = fll;
               

                var rating= 0;  
                if (fll.employeename_ICDR_Ratings != null && fll.employeename_ICDR_Ratings != undefined && fll.employeename_ICDR_Ratings != "") {
                    rating = parseFloat(fll.employeename_ICDR_Ratings);
                } else {
                    if ($scope.EmployeeDemoRatingList != null && $scope.EmployeeDemoRatingList.length > 0) {
                        angular.forEach($scope.EmployeeDemoRatingList, function (itm1) {
                            if (itm1.icdR_GivenTo_EmpId == fll.employeenamesch_HRME_Id) {
                                rating = itm1.icdR_Ratings;
                            }
                        });
                    }
                   
                    

                    
                }
                if (rating == 0) {
                    rating = 0.0;
                }
                var data = {
                    "HRME_Id": fll.employeenamesch_HRME_Id,
                    "Rating": rating
                }
                apiService.create("Sales_Lead/SwappingEmpList", data).
                    then(function (promise) {

                        if (promise.swappingEmpList.length > 0 && promise.swappingEmpList!= null && promise.swappingEmpList != undefined) {
                            $scope.sales_swappingEmpData = promise.swappingEmpList;
                            $scope.isModalVisibleSwap = true;
                            $('#SwapModel').modal('show');
                        }
                        else {
                            swal('There is no Employee Available for Swapping...');
                            $state.reload();
                        }
                    })



               
            }
        }

       
        //nagaraj added
        $scope.submitted = false;
        $scope.swapSave = function (datt) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSLEDM_IdNew": datt.ismsledM_Id,
                    "HRME_ID": $scope.HRME_Id.hrme_id
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Swap Scheduled To (Demo Assigned To) from " + datt.employeename + " to " + $scope.HRME_Id.EmployeeName + "... ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes,",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("Sales_Lead/Swap", data).
                                then(function (promise) {
                                    if (promise.msg === "swap") {
                                        swal('Employee Swapped Successfully !');
                                    }
                                    else if (promise.msg === "Not swap") {
                                        swal('Employee Not Swapped !');
                                    }
                                    else if (promise.msg === "Duplicate") {
                                        swal("Selected Employee Already Swapped For Same Lead Already....!");
                                    }
                                    else if (promise.msg === "Error") {
                                        swal("Please Contact Administrator !");
                                    }
                                    else {
                                        swal("Sorry...something went wrong");
                                    }
                                    $scope.sales_swappingEmpData = [];
                                    $('#SwapModel').modal('hide');
                                    $scope.LoadData();

                                })
                        } else {
                            swal("Cancelled");
                            $scope.sales_swappingEmpData = [];
                            $('#SwapModel').modal('hide');
                            $scope.LoadData();
                        }

                    });
            }
           
            else {
               // $scope.submitted = true;
                $scope.sales_swappingEmpData = [];
                $scope.LoadData();
               //$('#SwapModel').modal('hide');
            //   $scope.LoadData();
            }
        }



          //nagaraj added
        $scope.Cancel_Remarks = '';
        $scope.CancelReason = function (fl) {
            if (fl.selected) {
                $scope.fl = fl;
                $scope.isModalVisible = true;
                $('#CancelReasonModel').modal('show');
            }
        }


          //nagaraj added
        $scope.SaveCancelresponse = function (dat) {

            var data = {
                "ISMSLEDM_Id_Cancel": dat.ismsledM_Id,
                "CancelRemarks": $scope.Cancel_Remarks
            }
            apiService.create("Sales_Lead/CancelNew", data).
                then(function (promise) {
                    if (promise.msg == 'CancelNew') {
                        swal("Cancel Remarks Updated Successfully!");
                      
                    }
                    else if (promise.msg == "Not-Cancel") {
                        swal(" Please Select Cancel Check box");
                        
                    }
                    else {
                        swal("Sorry...something went wrong");
                       
                    }
                    $('#CancelReasonModel').modal('hide');
                    $scope.LoadData();
                })
           
    
        }
        //nagaraj added
        $scope.CancelNew = function (fl) {

            var data = {
                "ISMSLEDM_Id_Cancel": fl.ismsledM_Id
            }

            if (fl.selected) {
                swal({
                    title: "Are You Sure To Cancel",
                    text: "Do You Want To update As Cancel..?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ISMSLEDM_Id_Cancel": fl.ismsledM_Id
                            }
                            apiService.create("Sales_Lead/CancelNew", data).
                                then(function (promise) {
                                    if (promise.msg == 'CancelNew') {
                                        swal("Cancel Updated Successfully!");
                                        $state.reload();
                                    }
                                    else if (promise.msg == "Not-Cancel") {
                                        swal(" Please Select Verify Check box");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Sorry...something went wrong");
                                        $state.reload();
                                    }
                                })
                        }
                        else {
                            $state.reload();
                        }
                    });

            } else {
                $state.reload();
            }
        };




        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacteded = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;;
        };

        $scope.interactednew = function (field) {
            return $scope.submittednew || field.$dirty;;
        };

        //adedd
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


        $scope.cancelSwap = function () {
           // $scope.HRME_Id.hrme_id = '';
            $scope.HRME_Id = null;
            $('#SwapModel').modal('hide');
            $scope.LoadData();
        }

        $scope.close = function () {
            $state.reload();
        }
    }

})();



