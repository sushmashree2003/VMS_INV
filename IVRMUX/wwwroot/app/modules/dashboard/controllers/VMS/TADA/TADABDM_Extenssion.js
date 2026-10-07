(function () {
    'use strict';
    angular.module('app').controller('TADABDM_ExtenssionController', TADATourReportController);
    TADATourReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout', '$window'];
    function TADATourReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout, $window) {

        var paginationformasters = 10;
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.taskeffort = [];
        $scope.tasklist = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.report = false;
        $scope.ismeridian = true;
        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 5;
        $scope.searchtwo = "";
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.onpageload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;
            $scope.flagone = false;
            $scope.flagtwo = false;
            $scope.flagtwos = false;
            $scope.flagones = false;
            apiService.getURI("TADA_Reports/getalldata", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.deviationpercentage = promise.deviationpercentage;
                $scope.empname = promise.empname;
                $scope.roleflag = promise.roleflag;
                if ($scope.departmentdropdown.length > 0) {
                    angular.forEach($scope.departmentdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.departmentselectedAll = true;
                }
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = true;
                    });
                    $scope.checkall = true;
                }
                $scope.TourList();
            });
        };
        /* DEPARTMENT SELECTION */
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;

            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
            $scope.Deptselectiondetails();
        };
        /* DESIGNATION SELECTION */
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbxdesg = function (groupType) {
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.otherdaydatechange = function () {

            $scope.get_Report = null;
        };
        /* EMPLOYEE SELECTION */
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserEmp
            };

            apiService.create("TADA_Reports/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation !== null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                    if ($scope.designationdropdown.length > 0) {
                        angular.forEach($scope.designationdropdown, function (uem) {
                            uem.selected = true;
                        });
                        $scope.designationselectedAll = true;
                    }
                    $scope.Desgselectiondetails();
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                    $scope.get_Report = null;
                }
            });
        };

        $scope.Desgselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("TADA_Reports/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = true;
                        });
                        $scope.checkall = true;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                    $scope.get_Report = null;
                }
            });
        };
        /* DEVIATION REPORT */


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };
        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
        };

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };
        $scope.exportToExcel = function (export_id) {
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "Task Enhancement Time table";
            var reportname = "#printsection";
            excelname = excelsheetname;
            excelname = excelname.toUpperCase() + '.xls';
            exportHref = Excel.tableToExcel(reportname, excelsheetname);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printsection").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.searchchkbx1 = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            angular.forEach($scope.tasklist, function (itm) {
                itm.selected = checkStatus;
            });
        };
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.tasklist.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.tasklist.some(function (options) {
                return options.selected;
            });
        };
        $scope.TourList = function () {
            $scope.tasklist = [];
            $scope.emplist = [];
            angular.forEach($scope.get_userEmplist, function (d) {
                if (d.empck === true) {
                    $scope.emplist.push({ HRME_Id: d.HRME_Id });
                }
            });
            if ($scope.emplist != null && $scope.emplist.length > 0) {
                var data = {
                    "filelist": $scope.emplist,
                    "flag": "1",

                }
                apiService.create("TADA_Advance_Tour/Get_TourList", data).then(function (promise) {

                    if (promise.tour_Details !== null && promise.tour_Details.length > 0) {
                        //  $scope.tasklist = promise.tour_Details;
                        angular.forEach(promise.tour_Details, function (d) {
                            if (d.VTADAAA_StatusFlg == "Approved") {
                                $scope.tasklist.push(d);
                            }
                        });

                    }
                    else {
                        swal("No Record Found !");

                    }
                });
            }
            else {
                swal("Select Atleast One Emolyee !");
            }
        }
        $scope.LeadsData = function (bil) {
            $scope.LeadComments = [];
            var data = {
                "IER_ID": bil.IER_ID,
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

        //added
        $scope.getReport = function () {
            //TADA_Advance_Tour
            $scope.VTADAA_TotalAppliedAmount = 0; $scope.VTADAA_BalanceHRMS = 0;
            $scope.leadLists = [];
            $scope.getArrayDc = []; $scope.EditArrayTwo = []; $scope.TourPlanDetails = []; $scope.FinalSumbmission = []
            $scope.ApprovalSubmission = [];
            var data = {                "IER_ID": $scope.IER_ID.IER_ID,            };            apiService.create("TADA_Advance_Tour/FinalSubMission", data).then(function (promise) {                var ILRT_Cost = 0;                if (promise.getArray != null && promise.getArray.length > 0) {                    $scope.getArrayDc = promise.getArray;
                }                if (promise.editArrayTwo != null && promise.editArrayTwo.length > 0) {
                    $scope.EditArrayTwo = promise.editArrayTwo;
                }                if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {                    $scope.TourPlanDetails = promise.tourPlanDetails;
                    $scope.startdate = new Date(promise.tourPlanDetails[0].VTADAAA_FromDate);
                    $scope.enddate = new Date(promise.tourPlanDetails[0].VTADAAA_ToDate);
                    $scope.VTADAAA_Id = promise.tourPlanDetails[0].VTADAAA_Id;

                }                //getloadarray                if (promise.getloadarray != null && promise.getloadarray.length > 0) {

                    $scope.materaldocuuploadNew = promise.getloadarray;

                }                //LeadLists                if (promise.leadLists != null && promise.leadLists.length > 0) {                    $scope.leadLists = promise.leadLists;
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
                }                //FinalSumbmission //ApprovalSubmission                if (promise.finalSumbmission != null && promise.finalSumbmission.length > 0) {                    $scope.FinalSumbmission = promise.finalSumbmission;
                }                if (promise.approvalSubmission != null && promise.approvalSubmission.length > 0) {                    $scope.ApprovalSubmission = promise.approvalSubmission;
                    // $scope.VTADAA_TotalAppliedAmount = promise.approvalSubmission[0].EligibleAmtToApply;

                    $scope.VTADAA_BalanceHRMS = promise.approvalSubmission[0].SalaryCreditAmt;
                    if ($scope.VTADAA_BalanceHRMS > 0) {

                    }
                    else {
                        $scope.VTADAA_TotalAppliedAmount = (Number(promise.approvalSubmission[0].TotalAmt) - (Number(promise.approvalSubmission[0].SactionedAmt))).toFixed(2);
                    }

                }                var counttotal1 = 0;
                var counttotal2 = 0;
                var counttotal3 = 0;
                $scope.TotalAppliedAmount = ""; $scope.TotalSactionedAmount = ""; $scope.TotalEligbleAmount = "";
                $scope.ILRT_BillCost = "";
                if ($scope.FinalSumbmission != null && $scope.FinalSumbmission.length > 0) {
                    angular.forEach($scope.FinalSumbmission, function (d) {

                        counttotal1 += d.VTADAAA_TotalAppliedAmount;
                        counttotal2 += d.VTADAAA_TotalSactionedAmount;
                        counttotal3 += d.VTADAAA_TotalEligbleAmount;

                    });                    $scope.TotalAppliedAmount = counttotal1                    $scope.TotalSactionedAmount = counttotal2                    $scope.TotalEligbleAmount = counttotal3
                }
                if (promise.getCategoryArray != null && promise.getCategoryArray.length > 0) {                    $scope.GetCategoryArray = promise.getCategoryArray;
                }                $scope.ILRT_BillCost = ILRT_Cost;            });

        }
        $scope.IMRC_CategoryId = "";
        $scope.Onchangecategory = function () {
            $scope.LeadList = [];
            if ($scope.IMRC_CategoryId > 0) {
                $scope.HRME_IdNew = "";
                angular.forEach($scope.tasklist, function (dd) {
                    if (dd.IER_ID == $scope.IER_ID.IER_ID) {
                        $scope.HRME_IdNew = dd.IER_EmpId;
                    }
                });
                var data = {
                    "IMRC_CategoryId": $scope.IMRC_CategoryId,
                    "HRME_Id": $scope.HRME_IdNew
                };
                apiService.create("TADAAdvanceApproval/TadaAllwence", data).then(function (promise) {
                    $scope.LeadList = promise.leadList;
                });
            }

        };
        $scope.OnchageExtraAmount = function () {
            if ($scope.obj.VTADAA_TotalExtra > 0) {
                $scope.VTADAA_TotalOverall = (Number($scope.obj.VTADAA_TotalExtra) + (Number($scope.VTADAA_TotalAppliedAmount)));
            }
            else {
                $scope.VTADAA_TotalOverall = $scope.VTADAA_TotalAppliedAmount;
            }
        }
        //TotalExtra

        $scope.Datacategory = [];
        $scope.LeadListTemp = [];
        $scope.AddToCart = function () {
            $scope.IMRC_CategoryName = "";
            $scope.Datacategorytemp = [];

            if ($scope.GetCategoryArray != null && $scope.GetCategoryArray.length > 0 && $scope.IMRC_CategoryId > 0) {
                angular.forEach($scope.GetCategoryArray, function (dd) {
                    if (dd.IMRC_CategoryId == $scope.IMRC_CategoryId) {
                        $scope.IMRC_CategoryName = dd.IMRC_CategoryName;
                    }
                })

                angular.forEach($scope.LeadList, function (dd) {
                    if (dd.checked == true) {
                        $scope.Datacategorytemp.push({
                            ISMSLE_Id: dd.ISMSLE_Id,
                            ISMSLE_LeadName: dd.ISMSLE_LeadName,
                            ISMSLE_StudentStrength: dd.ISMSLE_StudentStrength,
                            ISMSLE_StaffStrength: dd.ISMSLE_StaffStrength,
                            ISMSMPR_ProductName: dd.ISMSMPR_ProductName,
                            SourceName: dd.SourceName,
                            IMRC_CategoryId: $scope.IMRC_CategoryId,
                            IVRMMS_Name: dd.IVRMMS_Name,
                            IMRC_CategoryName: $scope.IMRC_CategoryName,
                            ISMSMPR_Id: dd.ISMSMPR_Id,
                            IVRMMS_Id: dd.IVRMMS_Id
                        });
                        // $scope.LeadListTemp.push(dd);

                    }
                })
                if ($scope.Datacategorytemp != null && $scope.Datacategorytemp.length > 0) {
                    angular.forEach($scope.Datacategorytemp, function (dd) {
                        $scope.Datacategory.push(dd);
                    })
                    $scope.LeadListNew = [];
                    $scope.LeadListNew = $scope.LeadList;
                    $scope.LeadList = [];
                    $scope.IMRC_CategoryId = "";
                }
                else {
                    swal("Please Select Atleast One Checkbox !")
                }

            }
            else {
                swal("Please Select Category !")
            }
            //$scope.scroll();
        }
        $scope.RemoveY = function (index) {
            var newItemNo = $scope.Datacategory.length - 1;
            $scope.Datacategory.splice(index, 1);
        }
        $scope.cancel = function () {

            $state.reload();

        }
        $scope.previewimg_new = function (img) {            $scope.ilrT_FilePath = img;            var img = $scope.ilrT_FilePath;            if (img != null) {                var imagarr = img.split('.');                var lastelement = imagarr[imagarr.length - 1];                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {                    $('#tadapreviewX').attr('src', $scope.ilrT_FilePath);                    $('#myimagePreviewtada').modal('show');                }                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {                    $window.open($scope.ilrT_FilePath);                }                else if (lastelement == 'pdf') {                    $('#showpdf').modal('hide');                    var imagedownload1 = "";                    imagedownload1 = $scope.ilrT_FilePath;                    $http.get(imagedownload1, { responseType: 'arraybuffer' })                        .success(function (response) {                            var fileURL = "";                            var file = "";                            var embed = "";                            var pdfId = "";                            file = new Blob([(response)], { type: 'application/pdf' });                            fileURL = URL.createObjectURL(file);                            pdfId = document.getElementById("pdfIdzz");                            pdfId.removeChild(pdfId.childNodes[0]);                            embed = document.createElement('embed');                            embed.setAttribute('src', fileURL);                            embed.setAttribute('type', 'application/pdf');                            embed.setAttribute('width', '100%');                            embed.setAttribute('height', '1000');                            pdfId.appendChild(embed);                            $('#showpdf').modal('show');                        });                }            }            else {                $window.open($scope.ilrT_FilePath)            }        };
        $scope.SaveTada = function () {
            var totalamount = $scope.txt_approxTotAmount;
            var VTADAAA_Id = 0;
            $scope.Datacategorysave = [];
            if ($scope.VTADAAA_Id > 0) {
                VTADAAA_Id = $scope.VTADAAA_Id;
            }
            if ($scope.Datacategory != null && $scope.Datacategory.length > 0) {
                angular.forEach($scope.Datacategory, function (dd) {
                    $scope.Datacategorysave.push({
                        ClientId: dd.ISMSLE_Id,
                        CategoryId: dd.IMRC_CategoryId,
                        ISMSMPR_Id: dd.ISMSMPR_Id
                    })
                })
            }
            
            var data = {
                
                "VTADAAA_TotalAppliedAmount": $scope.obj.VTADAA_TotalExtra,                           
                "VTADAAA_Id": VTADAAA_Id,
                "listEmployeeSales": $scope.Datacategorysave,
                "message": "SaveAuto",
                "PlanStartDate": new Date($scope.startdate).toDateString(),
                "PlanEndDate": new Date($scope.enddate).toDateString(),

                
            };
            apiService.create("TADA_Advance_Tour/Edit_response_lead_demo", data).then(function (promise) {
                if (promise.returnvalue === true) {
                    if (promise.returnval === "Insert") {
                        swal("Record Saved Successfully", "", "success");
                    }
                    if (promise.returnval === "Failed") {
                        swal("Record Not saved", "", "info");
                    }
                    if (promise.returnval === "Duplicate") {
                        swal("Record Already Exist", "", "warning");
                    }
                    if (promise.returnval === "Update") {
                        swal("Record Update Successfully", "", "success");
                    }
                    if (promise.returnval === "UpdateFailed") {
                        swal("Record Not Update");
                    }
                    if (promise.returnval === "") {
                        swal("please contact administrator !");
                    }

                }
                else {
                    swal("please contact administrator !");
                }
                $state.reload();

            });

        }
    }
})();

