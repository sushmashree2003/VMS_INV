(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_Target_MappingController', Lead_Target_MappingController)

    Lead_Target_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile', '$sce']
    function Lead_Target_MappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile, $sce) {
        $scope.clientmailidEmail = "";
        $scope.ApprovedstatusNew = "";
        $scope.obj = {};
        // $scope.Dept = {};
        $scope.sortReverse = true;
        $scope.editflag = false;
        $scope.updateflag = false;
        $scope.date = new Date();
        $scope.obj.apprvalFlag = false;
        $scope.Resendflag = false;
        $scope.ISMSLET_Status = ""; $scope.ismsboM_BOMName = "";
        var paginationformasters;
        $scope.bomList = []; $scope.newcaste = {}; $scope.product_Temp = []; $scope.bomListNew = []; $scope.PListNew = [];
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.searchValueDeptd = "";
        $scope.obj.SilverPackage = "";
        $scope.obj.GoldPackage = "";
        $scope.obj.GoldSilverPackage = "";

        $scope.searchvalues = ""; $scope.leadTempDetails = [];

        $scope.serachParks = "";
        //getleadtarget
        $scope.getAllleadtarget = function () {

            //$scope.presentCountgrid = "";
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;
            var id = 2;

            apiService.getURI("Lead_Target_Mapping/OnloadGetDetails", id).
                then(function (promise) {

                    if (promise.leadlist != null && promise.leadlist.length > 0) {
                        $scope.leadlist = promise.leadlist;
                    }
                    if (promise.bomfinallist != null && promise.bomfinallist.length > 0) {
                        $scope.leadTempDetails = promise.bomfinallist;
                    }
                    //$scope.bomfinallist = promise.bomfinallist;


                });
        };
        ///////////////////////////////

        $scope.OnChangelead = function () {
            $scope.BomListNewupdate = [];
            if ($scope.leadlist != null && $scope.leadlist.length > 0 && $scope.leadTempDetails != null && $scope.leadTempDetails.length > 0 && $scope.Resendflag == false) {
                angular.forEach($scope.leadTempDetails, function (dd) {
                    if ($scope.Dept.ISMSLE_Id == dd.PEP_ISMSLE_ID) {
                       
                        return;
                    }
                });

            }

            var data = {
                "ISMSLE_Id": $scope.Dept.ISMSLE_Id,
                "flag": "LeadDetails"
            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeleadlist", data).then(function (promise) {

                if (promise.selectedleadlist != null && promise.selectedleadlist.length > 0) {
                    $scope.ISMSLE_LeadName = promise.selectedleadlist[0].ISMSLE_LeadName;
                    $scope.ISMSLE_ContactNo = promise.selectedleadlist[0].ISMSLE_ContactNo;
                    $scope.ISMSLE_EmailId = promise.selectedleadlist[0].ISMSLE_EmailId;
                    $scope.ISMSLE_ContactPerson = promise.selectedleadlist[0].ISMSLE_ContactPerson;
                    $scope.ISMSMCA_CategoryName = promise.selectedleadlist[0].ISMSMCA_CategoryName;
                    $scope.Before_employeeName = promise.selectedleadlist[0].Before_employeeName;

                    $scope.ISMSLE_LeadAddress1 = promise.selectedleadlist[0].ISMSLE_LeadAddress1;
                    $scope.ISMSLE_StudentStrength = promise.selectedleadlist[0].ISMSLE_StudentStrength;
                    $scope.ISMSLE_StaffStrength = promise.selectedleadlist[0].ISMSLE_StaffStrength;
                    $scope.State = promise.selectedleadlist[0].State;
                    $scope.Country = promise.selectedleadlist[0].Country;
                    $scope.Status1 = promise.selectedleadlist[0].Status;

                }

                if (promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                    $scope.BomListNewupdate = promise.bomTemplateList;
                }
                if (promise.productlist != null && promise.productlist.length > 0) {
                    $scope.productlist = promise.productlist;
                    angular.forEach($scope.productlist, function (dd) {
                        dd.bomListNew = [];
                        angular.forEach($scope.BomListNewupdate, function (ddd) {
                            dd.bomListNew.push(ddd);

                        });
                    });
                }


                if (promise.sendQuotationleadlist != null && promise.sendQuotationleadlist.length > 0) {
                    $scope.sendQuotationleadlist = promise.sendQuotationleadlist;
                    if ($scope.PListNew != null && $scope.PListNew.length > 0) {//d.checked
                        angular.forEach($scope.sendQuotationleadlist, function (ddd) {
                            angular.forEach($scope.PListNew, function (dd) {
                                if (ddd.ISMSMPR_Id == dd.ISMSMPR_Id) {
                                    ddd.checked = true;
                                }
                            });
                        });
                    }

                } else {
                    swal('No Data Found ');
                    $state.reload();
                }

            });
        };


        ///////////////////////////////////


        $scope.OnChangeproduct = function () {
            $scope.bomList = []; var ISMSLE_Id = 0;
            //$scope.bomListNew = []; $scope.PListNew = [];
            var products = [];
            if ($scope.PListNew != null && $scope.PListNew.length > 0) {
                angular.forEach($scope.PListNew, function (d) {
                    products.push(d.ISMSMPR_Id);
                });
            }
            else {
                angular.forEach($scope.sendQuotationleadlist, function (d) {
                    if (d.checked) {
                        products.push(d.ISMSMPR_Id);
                    }
                });
            }
            if ($scope.Resendflag == false) {
                ISMSLE_Id = $scope.Dept.ISMSLE_Id;
            }


            var data = {
                "product": products,
                "ISMSLE_Id": ISMSLE_Id,
            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeproductlist", data).then(function (promise) {

                if (promise.bomList != null && promise.bomList.length > 0) {
                    $scope.bomList = promise.bomList;//selected
                    if ($scope.bomListNew != null && $scope.bomListNew.length > 0) {
                        angular.forEach($scope.bomList, function (ddd) {
                            angular.forEach($scope.bomListNew, function (dd) {
                                if (ddd.ISMSBOM_Id == dd.ISMSBOM_Id) {
                                    ddd.selected = true;
                                }
                            });
                        });
                    }


                } else {
                    swal('No Data Found ');
                    //$state.reload();
                    $scope.ismsboM_BOMName = "";
                }

            });
        };


        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.checked = toggleStatus;
            });

            $scope.OnChangeproduct();
        };
        $scope.togchkbx = function (optionnew) {
            $scope.checkall = $scope.sendQuotationleadlist.every(function (options) {
                return options.checked;
            });

            angular.forEach($scope.sendQuotationleadlist, function (uem) {
                if (optionnew.ISMSMPR_ProductName == uem.ISMSMPR_ProductName) {
                }
                else {
                    uem.checked = false;
                }

            });
            $scope.OnChangeproduct();

        };

        $scope.searchValueE = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.ISMSMPR_ProductName).indexOf(angular.lowercase($scope.searchValueE)) >= 0;
        };
        ///////////////////////
        $scope.all_checks = function (ack) {
            $scope.checkalls = ack;
            var toggleStatuss = $scope.checkalls;
            angular.forEach($scope.filterValue4, function (uem) {
                uem.selected = toggleStatuss;
            });
            $scope.OnChangeBOMList();

        };
        $scope.togchkbox = function (optionnew) {
            $scope.checkalls = $scope.bomList.every(function (options) {
                return options.selected;
            });
            angular.forEach($scope.bomList, function (uem) {
                if (optionnew.ISMSBOM_BOMName == uem.ISMSBOM_BOMName) {
                }
                else {
                    uem.selected = false;
                }

            });
            $scope.OnChangeBOMList();
        };

        $scope.searchValueEE = '';
        $scope.filterValue5 = function (obj) {
            return angular.lowercase(obj.ISMSBOM_BOMName).indexOf(angular.lowercase($scope.searchValueEE)) >= 0;
        };


        $scope.OnChangeBOMList = function (edits) {
            // $scope.bomList = [];
            $scope.selectedleadlist = []; var ISMSLET_Id = 0; $scope.leadloaddata = []; $scope.perstudentDetails123 = [];
            $scope.amt = ""; $scope.obj.VMSMOUT_Flag = ""; $scope.obj.ScincePark = 0;
            $scope.groupedData = [];
            $scope.amt_4 = ""; $scope.ismslE_Pincode = ""; $scope.ivrmmS_Name = ""; $scope.ivrmmcT_Name = "";
            $scope.obj.ISMSLET_Amount_I = ""; $scope.obj.empName = ""; $scope.paymentterms = [];
            $scope.obj.ISMSLET_Amount_Final = ""; $scope.editleadtemp = []; $scope.ModuleName = [];
            var BOMLists = []; $scope.DLC = ""; $scope.DLHC = ""; $scope.PMC = ""; $scope.HRC = ""; $scope.addres1 = ""; $scope.addres2 = "";
            if ($scope.bomListNew != null && $scope.bomListNew.length > 0) {
                angular.forEach($scope.bomListNew, function (d) {
                    BOMLists.push(d.ISMSBOM_Id);
                });
            }
            else {
                angular.forEach($scope.bomList, function (d) {
                    if (d.selected) {
                        BOMLists.push(d.ISMSBOM_Id);
                        $scope.ISMSBOM_BOMNameCSM = d.ISMSBOM_BOMName

                    }
                });
            }
            if (edits != null && edits != "") {
                BOMLists.push(edits.PEP_ISMSBOM_Id);
                ISMSLET_Id = edits.PEP_Id;
                $scope.ISMSBOM_BOMNameCSM = edits.ISMSBOM_BOMName;
                $scope.Dept.ISMSLE_Id = edits.PEP_ISMSLE_ID;

                $scope.ApprovedstatusNew = edits.PEP_Status;
            }
            var data = {
                "BOMListt": BOMLists,
                "ISMSLE_Id": $scope.Dept.ISMSLE_Id,
                "ISMSLET_Id": ISMSLET_Id,
                "ISMSLBOM_BOMFlg": $scope.ISMSBOM_BOMNameCSM

            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeBOMList", data).then(function (promise) {

                $scope.ApprovedstatusNew = $scope.ApprovedstatusNew;

                if (promise.ismslboM_BOMFlg == "Proposal Customization") {
                    if (promise.bomfinallist != null && promise.bomfinallist.length > 0) {
                        angular.forEach(promise.bomfinallist, function (d) {

                            if (d.VMSMOUT_Name == promise.ismslboM_BOMFlg) {
                                var e1 = angular.element(document.getElementById("LeadIntimate"));
                                $scope.xtra = d.VMSMOUT_Template;
                                e1.html($scope.xtra);
                                $compile(e1.contents())($scope);
                                $scope.obj.VMSMOUT_Flag = d.VMSMOUT_Flag;
                                
                                return;
                            }
                        });



                    }


                    if (promise.bomTemplateList != undefined && promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                        $scope.bomTemplateList = promise.bomTemplateList;
                        $scope.ismslE_LeadName = $scope.bomTemplateList[0].ISMSLE_LeadName;
                        $scope.ismsmpR_ProductName = $scope.bomTemplateList[0].ISMSMPR_ProductName;
                        $scope.ismslE_ContactDesignation = $scope.bomTemplateList[0].ISMSLE_ContactDesignation;
                        $scope.addres = $scope.bomTemplateList[0].addres;
                        $scope.ismslE_ContactNo = $scope.bomTemplateList[0].ISMSLE_ContactNo;
                        $scope.ismslE_ContactPerson = $scope.bomTemplateList[0].ISMSLE_ContactPerson;
                        $scope.ismslE_EmailId = $scope.bomTemplateList[0].ISMSLE_EmailId;
                        $scope.fmscoR_RefernceNo = $scope.bomTemplateList[0].FMSCOR_RefernceNo;
                        $scope.hrmE_EmployeeFirstName = $scope.bomTemplateList[0].HRME_EmployeeFirstName;
                        $scope.ismspS_Service_Name = $scope.bomTemplateList[0].ISMSPS_Service_Name;
                        $scope.ismslE_Id = $scope.bomTemplateList[0].ISMSLE_Id;

                        $scope.addres = promise.bomTemplateList[0].addres;
                        $scope.addres1 = promise.bomTemplateList[0].addres1;
                        $scope.addres2 = promise.bomTemplateList[0].addres2;
                        $scope.ismslE_Pincode = promise.bomTemplateList[0].ISMSLE_Pincode;
                        $scope.ivrmmS_Name = promise.bomTemplateList[0].IVRMMS_Name;
                        $scope.ivrmmcT_Name = promise.bomTemplateList[0].IVRMMCT_Name;
                        $scope.ismslE_EmailId = promise.bomTemplateList[0].ISMSLE_EmailId;

                        $scope.ISMSLET_Status = "";
                    }













                    if ((promise.perstudent_QutationReq != undefined && promise.perstudent_QutationReq != null && promise.perstudent_QutationReq.length > 0) ||
                        (promise.paymentterms != undefined && promise.paymentterms != null && promise.paymentterms.length > 0)
                        || (promise.selectedleadlist != undefined && promise.selectedleadlist != null && promise.selectedleadlist.length > 0)) {

                        $scope.Moudetails = promise.perstudent_QutationReq;
                        if ($scope.Moudetails != undefined && $scope.Moudetails != null && $scope.Moudetails.length > 0) {
                            $scope.ISMMCLTPR_MOURefNo = $scope.Moudetails[0].ISMMCLTPR_MOURefNo;
                            $scope.ProductCnt = $scope.Moudetails[0].ProductCnt;
                            $scope.CustumCnt = $scope.Moudetails[0].CustumCnt;

                        }




                        if (Array.isArray($scope.Moudetails)) {
                            for (let i = 0; i < $scope.Moudetails.length; i++) {
                                if ($scope.Moudetails[i].ISMMCLTPR_MOURefNo) {
                                    $scope.ISMMCLTPR_MOURefNo = $scope.Moudetails[i].ISMMCLTPR_MOURefNo;
                                    $scope.ProductCnt = $scope.Moudetails[i].ProductCnt;
                                    $scope.CustumCnt = $scope.Moudetails[i].CustumCnt;
                                    $scope.ClauseNo = $scope.Moudetails[i].ClauseNo;
                                    break;
                                }
                            }
                        }

                        var productmanhour = 0;
                        var clientmanhour = 0;
                        var clientrateperday = 0;
                        var clienttotalamt = 0;
                    

                        $scope.ProductChanges = promise.paymentterms;
                        $scope.ClientChanges = promise.selectedleadlist;

                        if ($scope.ProductChanges != undefined && $scope.ProductChanges != null && $scope.ProductChanges.length > 0) {

                            angular.forEach($scope.ProductChanges, function (ccc) {
                                productmanhour = productmanhour + Math.round(Number(ccc.IVRMVRT_ManHour));

                            })
                        }
                        
                        $scope.Totalproductchnages = productmanhour;

                        if ($scope.ClientChanges != undefined && $scope.ClientChanges != null && $scope.ClientChanges.length > 0) {
                            angular.forEach($scope.ClientChanges, function (cc) {
                                clientmanhour = clientmanhour + Math.round(Number(cc.IVRMVRT_ManHour));
                                clientrateperday = clientrateperday + Math.round(Number(cc.RatePerDay));
                                clienttotalamt = clienttotalamt + Math.round(Number(cc.TotalCost));
                            })
                        }


                        $scope.totclientmanhour = clientmanhour;
                        $scope.totclientrateperday = clientrateperday;
                        $scope.totclienttotalamt = clienttotalamt ;


                    } else {
                        swal("No Data Found");
                        $state.reload();
                    }


                    //$scope.Moudetails = promise.perstudent_QutationReq;
                    //if ($scope.Moudetails != undefined && $scope.Moudetails != null && $scope.Moudetails.length > 0) {
                    //    $scope.ISMMCLTPR_MOURefNo = $scope.Moudetails[0].ISMMCLTPR_MOURefNo;
                    //    $scope.ProductCnt = $scope.Moudetails[0].ProductCnt;
                    //    $scope.CustumCnt = $scope.Moudetails[0].CustumCnt;

                    //}
                 

                    

                    //if (Array.isArray($scope.Moudetails)) {
                    //    for (let i = 0; i < $scope.Moudetails.length; i++) {
                    //        if ($scope.Moudetails[i].ISMMCLTPR_MOURefNo) {
                    //            $scope.ISMMCLTPR_MOURefNo = $scope.Moudetails[i].ISMMCLTPR_MOURefNo;
                    //            $scope.ProductCnt = $scope.Moudetails[i].ProductCnt;
                    //            $scope.CustumCnt = $scope.Moudetails[i].CustumCnt;
                    //            break;
                    //        }
                    //    }
                    //}




                    //$scope.ProductChanges = promise.paymentterms;
                    //$scope.ClientChanges = promise.selectedleadlist;

               






                } 
                    
           
            });
        };


        $scope.addgrnrows = function (data) {
            if (data.bomTemplateList.length > 1) {
                for (var i = 0; i === data.bomTemplateList.length; i++) {
                    var id = data.bomTemplateList[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            data.bomTemplateList.push({ 'itrS_Id': 'trans' + newItemNo });
        };

        $scope.bomTemplateList = [{ itrS_Id: 'trans1' }];
        if ($scope.bomTemplateList.length === 1) {
            $scope.cnt = 1;
        }

        $scope.removegrnrows = function (index, data) {
            var newItemNo = data.bomTemplateList.length - 1;
            data.bomTemplateList.splice(index, 1);

        };
        //https://vapsclg.blob.core.windows.net/files/PrintPdf_Bootstrap.css
        //https://vapsclg.blob.core.windows.net/files/printpdf.css


        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionIdmail").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }



        //save
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.productlist = [];
            $scope.bomlists = [];
            $scope.serviceitamlist = [];
            angular.forEach($scope.taskid5, function (dd) {
                angular.forEach(dd.bomTemplateList, function (ddd) {
                    $scope.serviceitamlist.push({
                        ISPSIF_Id: ddd.ISPSIF_Id,
                        ISMSPS_ID: dd.ISMSPS_ID,
                        ISMSMPR_Id: ddd.ISMSMPR_Id,
                        ISMSBOM_Id: ddd.ISMSBOM_Id,
                        ISPSIF_ItemsName: ddd.ISPSIF_ItemsName,
                        ISPSIF_Quantity: ddd.ISPSIF_Quantity,
                        ISPSIF_Cost: ddd.ISPSIF_Cost
                    })
                });
            });
            $scope.amt = $scope.sales_CostPerMonth[0].AMt;
            $scope.amt_4 = $scope.sales_CostPerMonth[0].AMt_4;

            var data = {
                "ISMSLE_Id": $scope.Dept.ISMSLE_Id,
                "bomListdetails": $scope.serviceitamlist,
                "preAmount_I": $scope.amt,
                "PreAmount_Final": $scope.amt_4,
                "ISMSLET_Amount_I": $scope.obj.ISMSLET_Amount_I,
                "ISMSLET_Amount_Final": $scope.obj.ISMSLET_Amount_Final
            }
            apiService.create("Lead_Target_Mapping/Saveboomlistpackage", data).
                then(function (promise) {
                    $scope.updateflag = false;
                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();

                        }
                        else if (promise.retrunMsg === "Add") {
                            swal('Record Saved Successfully');
                            $scope.OnChangeBOMList();
                            //$state.reload();

                        }
                        else if (promise.retrunMsg === "Error") {
                            swal('Record Not Saved ');
                            return;
                        }

                        $scope.editflag = false;
                        $scope.ISMSLETARM_Id = 0;
                        $scope.submitted = false;
                        $scope.OnChangeLTM_Institution();

                    }

                    else {

                        swal('Operation Failed');
                        $state.reload();
                    }

                });
        }
        //OnChangeLTM_Institution
        $scope.scinceParksvalidation = function () {
            $scope.obj.SilverPackage = ""; $scope.obj.GoldSilverPackage = "";
            $scope.obj.PackageCount = ""; $scope.obj.GPackageCount = "";
            $scope.obj.GoldSilverPackage = "";
            var counts = 0; var countsGold = 0;
            if ($scope.ismsboM_BOMName == "Science Park Silver Package Outdoor" || $scope.ismsboM_BOMName == "Science Park Gold Package Outdoor") {
                if ($scope.perstudentDetails != null && $scope.perstudentDetails.length > 0) {
                    angular.forEach($scope.perstudentDetails, function (ddd) {
                        if (ddd.ISMSPSI_ItemsDescription == "1") {
                            $scope.obj.SilverPackage = ddd.ISPSIF_Quantity;
                        }

                    });
                    if ($scope.Outdoor_Id != null && $scope.Outdoor_Id.length > 0) {
                        angular.forEach($scope.Outdoor_Id, function (emp) {
                            angular.forEach(emp.Scinceparknames, function (dev) {
                                if (dev.ISMSSCM_Remark == "a)") {
                                    counts = counts + 1;
                                }
                            });
                        });
                    }
                }

                $scope.obj.PackageCount = counts;
                if ((Number($scope.obj.SilverPackage)) == (Number($scope.obj.PackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity !");
                    return;
                }

            }
            if ($scope.ismsboM_BOMName == "Science Park Silver && Gold Package Outdoor") {

                if ($scope.perstudentDetails != null && $scope.perstudentDetails.length > 0) {
                    angular.forEach($scope.perstudentDetails, function (ddd) {
                        if (ddd.ISMSPSI_ItemsDescription == "1") {
                            $scope.obj.GoldSilverPackage = ddd.ISPSIF_Quantity;
                        }
                        if (ddd.ISMSPSI_ItemsDescription == "4") {
                            $scope.obj.SilverPackage = ddd.ISPSIF_Quantity;

                        }
                    });
                    if ($scope.Outdoor_Id != null && $scope.Outdoor_Id.length > 0) {
                        angular.forEach($scope.Outdoor_Id, function (emp) {
                            angular.forEach(emp.Scinceparknames, function (dev) {
                                if (dev.ISMSSCM_Remark == "a)" && dev.ISMSSCM_Flag == "SP") {
                                    counts = counts + 1;
                                }
                                if (dev.ISMSSCM_Remark == "a)" && dev.ISMSSCM_Flag == "GP") {
                                    countsGold = countsGold + 1;
                                }
                            });
                        });
                    }
                }

                $scope.obj.GPackageCount = countsGold;
                $scope.obj.PackageCount = counts;
                if ((Number($scope.obj.SilverPackage)) == (Number($scope.obj.PackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity For Silver Pacakage !");
                    return;
                }
                if ((Number($scope.obj.GoldSilverPackage)) == (Number($scope.obj.GPackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity For Gold Pacakage !");
                    return;
                }
            }
            if ($scope.ismsboM_BOMName == "Science Park Silver Boot Model" || $scope.ismsboM_BOMName == "Science Park Gold Boot Model") {
                if ($scope.perstudentDetails != null && $scope.perstudentDetails.length > 0) {
                    angular.forEach($scope.perstudentDetails, function (ddd) {
                        if (ddd.ISMSPSI_ItemFlag == "SPMBM") {
                            $scope.obj.SilverPackage = ddd.ISPSIF_Quantity;
                        }
                        if (ddd.ISMSPSI_ItemFlag == "SPMBMN") {
                            $scope.obj.SilverPackage = ddd.ISPSIF_Quantity;
                        }
                    });
                    if ($scope.Outdoor_Id != null && $scope.Outdoor_Id.length > 0) {
                        angular.forEach($scope.Outdoor_Id, function (emp) {
                            angular.forEach(emp.Scinceparknames, function (dev) {
                                if (dev.ISMSSCM_Remark == "a)") {
                                    counts = counts + 1;
                                }
                            });
                        });
                    }
                }

                $scope.obj.PackageCount = counts;
                if ((Number($scope.obj.SilverPackage)) == (Number($scope.obj.PackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity !");
                    return;
                }
            }
            if ($scope.ismsboM_BOMName == "Science Park Boot Model") {

                if ($scope.perstudentDetails != null && $scope.perstudentDetails.length > 0) {
                    angular.forEach($scope.perstudentDetails, function (ddd) {
                        if (ddd.ISMSPSI_ItemFlag == "SPMBMN") {
                            $scope.obj.GoldSilverPackage = ddd.ISPSIF_Quantity;
                        }
                        if (ddd.ISMSPSI_ItemFlag == "SPMBM") {
                            $scope.obj.SilverPackage = ddd.ISPSIF_Quantity;

                        }
                    });
                    if ($scope.Outdoor_Id != null && $scope.Outdoor_Id.length > 0) {
                        angular.forEach($scope.Outdoor_Id, function (emp) {
                            angular.forEach(emp.Scinceparknames, function (dev) {
                                if (dev.ISMSSCM_Remark == "a)" && dev.ISMSSCM_Flag == "SPBM") {
                                    counts = counts + 1;
                                }
                                if (dev.ISMSSCM_Remark == "a)" && dev.ISMSSCM_Flag == "GPBM") {
                                    countsGold = countsGold + 1;
                                }
                            });
                        });
                    }
                }

                $scope.obj.GPackageCount = countsGold;
                $scope.obj.PackageCount = counts;
                if ((Number($scope.obj.SilverPackage)) == (Number($scope.obj.PackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity For Silver Pacakage !");
                    return;
                }
                if ((Number($scope.obj.GoldSilverPackage)) == (Number($scope.obj.GPackageCount))) {

                }
                else {
                    swal("Approval  Quantity  Is Not Matched Actual  Quantity For Gold Pacakage !");
                    return;
                }
            }
        }
        $scope.removeScincePark = function (index, dd) {
            angular.forEach($scope.Outdoor_Id, function (emp) {
                angular.forEach(emp.Scinceparknames, function (dev) {
                    if (dev.ISMSSCM_Remark == "a)" && dev.ISMSSCT_Id == dd.ISMSSCT_Id) {
                        emp.Scinceparknames.splice(index, 1);
                        $scope.scinceParksvalidation();
                        return;
                    }
                });
            });

        };
        $scope.SendMail = function (Mail) {
            var mgs = ""; var retrunwhatsup = "whatsup"; $scope.bomListdetails = [];
            if (Mail == 'clientmail') {
                mgs = $scope.ismslE_EmailId;
                mgs = $scope.clientmailidEmail;

            }
            else {
                //mgs = "mktgcoord@vapstech.com";
                 mgs = "praveenkumar.b@vapstech.com";

                //mgs = "nagaraja.mv@vapstech.com";
            }
            e
            swal({
                title: "Kindly check Lead Email?" + mgs + "",
                text: "If Lead Email Not Correct...Go To Lead_Creation Page and Update Email",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Save it",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.tmplt = [];
                        $scope.doc_list2 = [];
                        var Template = document.getElementById("printSectionIdmail").innerHTML;
                        //var Templates = document.getElementById("printSectionIdmailss").innerHTML;


                        var Templates = document.getElementById("printSectionIdmail").innerHTML;
                        var LeadIntimate = document.getElementById("LeadIntimate").innerHTML;
                        $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

                        //added new things
                        if ($scope.ismsmpR_ProductName == "VAPS  AI  DIGITAL") {
                            $scope.ismsmpR_ProductName = "AI  DIGITAL";
                        }
                        else {
                            $scope.ismsmpR_ProductName = $scope.ismsmpR_ProductName;
                        }

                        var data = {
                            "ISMSMOU_Packagename": 'ProposalCustomization',
                            //"ManagerMailid": mgs,
                            "ManagerMailid": 'praveenkumar.b@vapstech.com',
                            "ISMSLE_Id": $scope.ismslE_Id,
                            Template: Template,
                            Templates: Templates,
                            "ISMSLET_HtmlName": 'ProposalCustomization',
                            ISMSLET_HtmlTemplate: LeadIntimate,
                            "retrunwhatsup": retrunwhatsup,
                            "ISMSLED_MobileNo": $scope.ismslE_ContactNo,
                            //"FirstName": 'ProposalCustomization',
                            "FirstName": 'Proposal for Customization',
                            "ISMSLET_Id": $scope.obj.ISMSLET_Id,
                            "bomListdetails": $scope.bomListdetails

                        };
                        apiService.create("Lead_Target_Mapping/sendmailDetails", data).
                            then(function (promise) {
                                swal('Email  Sent Successfully.');
                                $state.reload();
                            });
                    }

                });
        }

        //edit
        $scope.EditLeadtarget_mapping = function (lead) {

            var data = {

                "MI_Id": lead.mI_Id,
                "ISMSLETARM_Id": lead.ismsletarM_Id,
                "ISMSLETARM_ApprovedBy": lead.ismsletarM_ApprovedBy
            }

            apiService.create("Lead_Target_Mapping/LeadTargetMapping_Edit", data).then(function (promise) {

                $scope.MI_Id = promise.editLeadTarget_Mapping[0].mI_Id;
                $scope.ISMSLETARM_Id = promise.editLeadTarget_Mapping[0].ismsletarM_Id;
                $scope.ISMSLETARM_LeadApplFlg = promise.editLeadTarget_Mapping[0].ismsletarM_LeadApplFlg;
                $scope.ISMSLETARM_NoOfLeads = promise.editLeadTarget_Mapping[0].ismsletarM_NoOfLeads;
                $scope.ISMSLETARM_LeadPeriodicity = promise.editLeadTarget_Mapping[0].ismsletarM_LeadPeriodicity;
                $scope.ISMSLETARM_DemoApplFlg = promise.editLeadTarget_Mapping[0].ismsletarM_DemoApplFlg;
                $scope.ISMSLETARM_NoOfDemos = promise.editLeadTarget_Mapping[0].ismsletarM_NoOfDemos;
                $scope.ISMSLETARM_DemoPeriodicity = promise.editLeadTarget_Mapping[0].ismsletarM_DemoPeriodicity;
                $scope.ISMSLETARM_ProposalApplFlg = promise.editLeadTarget_Mapping[0].ismsletarM_ProposalApplFlg;
                $scope.ISMSLETARM_NoOfProposals = promise.editLeadTarget_Mapping[0].ismsletarM_NoOfProposals;
                $scope.ISMSLETARM_ProposalPeriodicity = promise.editLeadTarget_Mapping[0].ismsletarM_ProposalPeriodicity;
                $scope.ISMSLETARM_ClosureApplFlg = promise.editLeadTarget_Mapping[0].ismsletarM_ClosureApplFlg;
                $scope.ISMSLETARM_NoOfClosures = promise.editLeadTarget_Mapping[0].ismsletarM_NoOfClosures;
                $scope.ISMSLETARM_ClosurePeriodicity = promise.editLeadTarget_Mapping[0].ismsletarM_ClosurePeriodicity;
                $scope.ISMSLETARM_NegotiationFlg = promise.editLeadTarget_Mapping[0].ismsletarM_NegotiationFlg;
                $scope.ISMSLETARM_NegotiationPercentage = promise.editLeadTarget_Mapping[0].ismsletarM_NegotiationPercentage;
                $scope.ISMSLETARM_ApprovedBy = promise.editLeadTarget_Mapping[0].approvedBy;
                $scope.editflag = true;

            });
        };

        $scope.updateCheckbox2 = function () {
            // If checkbox1 is checked, disable checkbox2
            if ($scope.checkbox1) {
                $scope.checkbox1 = True; // Uncheck checkbox2
            }
        };




        $scope.Clearall = function () {
            $state.reload();
            $scope.editflag = false;

        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.onupdate = function () {
            $scope.bomListdetailsNew = []; $scope.bomListNew = []; $scope.ismsboM_BOMName = ""; $scope.obj.ISMSLET_Amount_I = ""; $scope.obj.ISMSLET_Amount_Final = "";
            if ($scope.productlist != null && $scope.productlist.length > 0) {
                angular.forEach($scope.productlist, function (pro) {
                    if (pro.ISMSBOM_Id > 0) {
                        $scope.bomListdetailsNew.push({
                            ISMSBOM_Id: pro.ISMSBOM_Id,
                            ISMSMPR_Id: pro.ISMSLEDMPR_Id,
                            ISMSLE_Id: pro.ISMSLE_Id,
                            ISMSMPR_ProductName: pro.ISMSMPR_ProductName
                        })
                        $scope.bomListNew.push({
                            ISMSBOM_Id: pro.ISMSBOM_Id,

                        })
                    }
                });
            }
            var ISMSLET_Id = 0;
            if ($scope.obj.ISMSLET_Id > 0) {
                ISMSLET_Id = $scope.obj.ISMSLET_Id;

            }
            else {
                $scope.bomListNew = [];
            }
            if ($scope.bomListdetailsNew != null && $scope.bomListdetailsNew.length > 0) {
                var data = {
                    "bomListdetails": $scope.bomListdetailsNew,
                    "ISMSLET_Id": ISMSLET_Id,
                }
                apiService.create("Lead_Target_Mapping/OnChangeLTM_Institution", data).then(function (promise) {
                    if (promise.retrunMsg == "Add") {
                        swal('Saved Successfully.');
                        angular.forEach($scope.bomList, function (d) {
                            d.selected = false;
                        });


                    }

                    else {
                        swal('Failed');

                    }
                });
            }
            else {
                swal("Select Package !");
            }
        };
        $scope.Clearid = function () {
            $state.reload();
        };


        //=====================view doc
        $scope.previewimg_new = function (img) {
            $('#myvideoPreview').modal('hide');
            $('#myimagePreview').modal('hide');
            $('#showpdf').modal('hide');
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'mp3') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)

            }

            else if ($scope.filetype2 == 'pdf') {
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
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                        document.getElementById("pdfIdzz").innerHTML = htmlElements;

                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };


        $scope.showprintdata = function (temp) {

            var imagedownload = temp.ISMSLET_HtmlPdf;
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


                });


            //$scope.ismsboM_BOMName = temp.ISMSLET_HtmlName;
            //var e1 = angular.element(document.getElementById("LeadIntimate"));
            //$compile(e1.html(temp.ISMSLET_HtmlTemplate))(($scope));
            //$('#myModal1').modal('show');

        }






        $scope.SendPro = function () {




            //$scope.productlist = [];
            //$scope.bomlists = [];
            $scope.ChangesDetails = [];
         
            //angular.forEach($scope.ClientChanges, function (ddd) {
            //    $scope.ChangesDetails.push({
            //            //ISPSIF_Id: ddd.ISPSIF_Id,
            //            ISMSPS_ID: ddd.RatePerDay,
            //            //ISMSMPR_Id: ddd.ISMSMPR_Id,
            //            //ISMSBOM_Id: ddd.ISMSBOM_Id, //
            //            ISPSIF_ItemsName: ddd.ISMTCR_Title, 
            //            ISPSIF_Quantity: ddd.IVRMVRT_ManHour, 
            //            ISPSIF_Cost: ddd.TotalCost,   
            //            ISMSMPR_ProductName:'CC'  
            //        })
            //});

            //angular.forEach($scope.ProductChanges, function (ddd) {
            //    $scope.ChangesDetails.push({
            //        //ISPSIF_Id: ddd.ISPSIF_Id,
            //       // ISMSPS_ID: '',
            //        //ISMSMPR_Id: ddd.ISMSMPR_Id,
            //        //ISMSBOM_Id: ddd.ISMSBOM_Id, //
            //        ISPSIF_ItemsName: ddd.ISMTCR_Title, 
            //        ISPSIF_Quantity: ddd.IVRMVRT_ManHour,  
            //        //ISPSIF_Cost: '',   
            //        ISMSMPR_ProductName:'PC'   
            //    })
            //});

            //ISPSIF_ItemsName: ddd.ISMTCR_Title, //title
            //   ISPSIF_Quantity: ddd.IVRMVRT_ManHour,   //manhour
            //   ISPSIF_Cost: '',   //totalcost
            //   ISMSMPR_ProductName = 'PC'   //product chnages




            //            public long ISPSIF_Id { get; set; }
            //public long ISMSMPR_Id { get; set; }
            //public long ISMSBOM_Id { get; set; }
            //public long ISMSPS_ID { get; set; }
            //public string ISPSIF_ItemsName { get; set; }
            //public decimal ISPSIF_Quantity { get; set; }
            //public decimal ISPSIF_Cost { get; set; }
            //public long  ISMSLE_Id { get; set; }
            //public string ISMSMPR_ProductName { get; set; }










            var BOMLists = [];
            var products = [];

            var Template = document.getElementById("printSectionIdmails").innerHTML;
            var LeadIntimate = document.getElementById("LeadIntimate").innerHTML;

            //  $scope.amt = $scope.sales_CostPerMonth[0].AMt;
            //  $scope.amt_4 = $scope.sales_CostPerMonth[0].AMt_4;
            angular.forEach($scope.bomList, function (d) {
                if (d.selected) {
                    BOMLists.push(d.ISMSBOM_Id);
                }
            });

            angular.forEach($scope.sendQuotationleadlist, function (d) {
                if (d.checked) {
                    products.push(d.ISMSMPR_Id);
                }
            });
            var preAmount_I = 0;
            var PreAmount_Final = 0;


            //if ($scope.ismsboM_BOMName == "E-Learning") {
            //    preAmount_I = 0;
            //    PreAmount_Final = 0;
            //    $scope.obj.ISMSLET_Amount_I = 0;
            //    $scope.obj.ISMSLET_Amount_Final = 0;
            //} else {
            //    if ($scope.obj.ISMSLET_Id > 0) {
            //        preAmount_I = $scope.ISMSLET_Amount_I;
            //        PreAmount_Final = $scope.ISMSLET_Amount_Final;
            //        $scope.obj.ISMSLET_Amount_I = 0;
            //        $scope.obj.ISMSLET_Amount_Final = 0;
            //    }
            //    else {
            //        preAmount_I = $scope.amt;
            //        PreAmount_Final = $scope.amt_4;
            //    }
            //}




            //if ($scope.obj.ISMSLET_Id > 0) {
            //    preAmount_I = $scope.ISMSLET_Amount_I;
            //    PreAmount_Final = $scope.ISMSLET_Amount_Final;
            //}
            //else {
            //    preAmount_I = $scope.amt;
            //    PreAmount_Final = $scope.amt_4;
            //}
            var data = {
                //"ISMSLET_Id": $scope.obj.ISMSLET_Id,
                "ISMSLE_Id": $scope.ismslE_Id,
                "flag": "ProposalCustomization",
                ISMSLET_HtmlTemplate: LeadIntimate,
                Template: Template,
                "product": products,
                "BOMListt": BOMLists,
                "ISMSLBOM_BOMDescription": $scope.fmscoR_RefernceNo,
                //"preAmount_I": preAmount_I,
                //"PreAmount_Final": PreAmount_Final,
                //"bomListdetails": $scope.ChangesDetails,
          
                "ISMSLE_StudentStrength": $scope.ISMSLE_StudentStrength
            };

            apiService.create("Lead_Target_Mapping/sendProDetails", data).then(function (promise) {
                if (promise.retrunMsg == "Add") {
                    swal('Saved Successfully.');
                    $state.reload();
                }
                else if (promise.retrunMsg == "Update") {
                    swal('Updated Successfully.');
                    $state.reload();
                }
                else {
                    swal('Failed');
                    $state.reload();
                }
            });
        };




        //-------------for active and deactive
        $scope.deactiveY = function (temp) {
            temp.ISMSBOM_BOMName = "ProposalCustomization";

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (temp.PEP_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ISMSLBOM_BOMName": 'ProposalCustomization',
                "ISMSLET_Id": temp.PEP_Id


            };



            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },

                
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("Lead_Target_Mapping/deactivate", data).
                            then(function (promise) {
                                if (promise.retrunMsg == "true") {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.edit = function (edits) {
            $scope.Resendflag = true;
            $scope.bomList = []; $scope.newcaste = {}; $scope.product_Temp = []; $scope.bomListNew = []; $scope.PListNew = [];
            // $scope.Dept.ISMSLE_Id = edits.ISMSLE_Id;
            for (var i = 0; i < $scope.leadlist.length; i++) {
                if ($scope.leadlist[i].ISMSLE_Id == edits.PEP_ISMSLE_ID) {
                    $scope.leadlist[i].Selected = true;
                    $scope.Dept = $scope.leadlist[i];
                    $scope.newcaste = $scope.leadlist[0].ISMSLE_Id;



                }
            }

            $scope.clientmailidEmail = edits.ISMSLE_EmailId;


            $scope.Dept.ISMSLE_Id = edits.PEP_ISMSLE_ID;
            $scope.OnChangelead();
            $scope.obj.ISMSLET_Id = edits.PEP_Id;
           
            $scope.OnChangeBOMList(edits);
            $scope.scroll();

        }

        //showprintdata


    }

})();