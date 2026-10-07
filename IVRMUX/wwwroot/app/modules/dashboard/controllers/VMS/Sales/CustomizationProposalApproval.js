(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_Target_MappingController', Lead_Target_MappingController)

    Lead_Target_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function Lead_Target_MappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {
        //$scope.years = "";
        //$scope.totalvalues = 0;
        $scope.ProductChanges = [];
        $scope.ClientChanges = [];
        $scope.showmodel = false;
        $scope.obj = {};
        $scope.years = 5;
        $scope.sortReverse = true;
        $scope.editflag = false;
        $scope.updateflag = false;
        $scope.date = new Date();
        $scope.selleadTempDetail = [];
        var paginationformasters = 0;
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
        //getleadtarget
        $scope.Dept = {}; $scope.leadTempDetails = [];
        // $scope.rdopunch = "BOTH";
        $scope.getAllleadtarget = function () {

            //$scope.presentCountgrid = "";
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            var id = 2;

            apiService.getURI("Lead_Target_Mapping/OnloadGetDetails", id).
                then(function (promise) {

                    if (promise.leadlist != null && promise.leadlist.length > 0) {
                        $scope.leadlist = promise.leadlist;
                    }
                    if (promise.confirmList != null && promise.confirmList.length > 0) {
                        $scope.leadTempDetails = promise.confirmList;
                        angular.forEach($scope.leadTempDetails, function (d) {
                            d.SciceParks = []; d.SciencePark_Boot = [];
                            d.ScinceParkflag = "";
                            d.flag = "";
                            d.flag = "Approved";
                        });
                    }
                    $scope.bomfinallist = promise.bomfinallist;


                });
        };
        ///////////////////////////////
        $scope.CalculateApproval = function (Cal) {
            var TotalCost = 0;
            if (Cal.ISMSLET_Amount_IMD > 0) {
                TotalCost = (Number((Cal.ISMSLET_Amount_IMD) * 12) * (Number(Cal.ISMSLE_StudentStrength)));
                Cal.ISMSLET_CostApproved = Math.round(Number(TotalCost));
                if (Number(Cal.ISMSLET_Amount_IMD) == (Number(Cal.Amount_I))) {
                    Cal.ISMSLET_CostApproved = Cal.ISMSLET_Cost;
                }
                if ((Cal.VMSMOUT_Name === 'Elite Package' || Cal.VMSMOUT_Name === 'Elite Plus Package') && Cal.ISMSLET_Amount_FinalMD > 0) {
                    if (Number(Cal.ISMSLET_4YrCost) < Number(Cal.ISMSLET_Amount_FinalMD)) {
                        var diffrence = Math.round(Number(Cal.ISMSLET_Amount_FinalMD)) - Math.round(Number(Cal.ISMSLET_4YrCost))
                        var TotalCost4yr = 0;
                        if (diffrence > 0) {

                            TotalCost4yr = (Number((diffrence) * 48) * (Number(Cal.ISMSLE_StudentStrength)));
                            TotalCost = Math.round(Number(TotalCost4yr) + Number(TotalCost));
                            Cal.ISMSLET_CostApproved = Math.round(Number(TotalCost));
                        }

                    }
                }

            }
        }

        $scope.OnChangelead = function () {
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
                }


                if (promise.sendQuotationleadlist != null && promise.sendQuotationleadlist.length > 0) {
                    $scope.sendQuotationleadlist = promise.sendQuotationleadlist;

                } else {
                    swal('No Data Found ');
                    $state.reload();
                }

            });
        };


        ///////////////////////////////////


        $scope.OnChangeproduct = function () {
            $scope.bomList = [];

            var products = [];
            angular.forEach($scope.sendQuotationleadlist, function (d) {
                if (d.checked) {
                    products.push(d.ISMSMPR_Id);
                }
            });

            var data = {
                "product": products,
                "ISMSLE_Id": 0
            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeproductlist", data).then(function (promise) {

                if (promise.bomList != null && promise.bomList.length > 0) {
                    $scope.bomList = promise.bomList;

                } else {
                    swal('No Data Found ');
                    $state.reload();
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
        $scope.togchkbx = function () {
            $scope.checkall = $scope.sendQuotationleadlist.every(function (options) {
                return options.checked;
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
        $scope.togchkbox = function () {
            $scope.checkalls = $scope.bomList.every(function (options) {
                return options.selected;
            });
            $scope.OnChangeBOMList();
        };

        $scope.searchValueEE = '';
        $scope.filterValue5 = function (obj) {
            return angular.lowercase(obj.ISMSBOM_BOMName).indexOf(angular.lowercase($scope.searchValueEE)) >= 0;
        };
        //AddToCart
        $scope.AddToPayments = function () {
            $scope.MtplotnScienceParkHistory = [];
            angular.forEach($scope.leadTempDetails, function (BOMLists) {
                if (BOMLists.ISMSLET_Id == $scope.Dept.ISMSLET_Id) {
                    var total = 0;
                    var totalNewhistory = 0;
                    angular.forEach($scope.perstudentDetails, function (d) {
                        var ISPSIF_Quantity = 0;
                        if (d.packageName == "M_Pacakage") {
                            ISPSIF_Quantity = d.ISPSIF_Quantitys;
                        }
                        else {
                            ISPSIF_Quantity = d.ISPSIF_Quantity;
                        }

                        if (BOMLists.ISMSLET_HtmlName == 'E-Learning') {

                            BOMLists.SciceParks.push({
                                ISPSIF_Id: d.ISPSIF_Id,
                                ISPSIF_Cost: d.ISPSIF_Cost,
                                ISMSLET_Id: BOMLists.ISMSLET_Id,
                                ISPSIF_Quantity: ISPSIF_Quantity,
                                ISPSIF_ItemsName: d.ISMSPSI_ItemsName
                            });

                        }
                        else if (BOMLists.ISMSLET_HtmlName == 'VAPS AI Camera') {
                            BOMLists.SciceParks.push({
                                ISPSIF_Id: d.ISPSIF_Id,
                                ISPSIF_Cost: d.ISPSIF_Costeach,
                                ISMSLET_Id: BOMLists.ISMSLET_Id,
                                ISPSIF_Quantity: ISPSIF_Quantity,
                                ISPSIF_ItemsName: d.ISMSPSI_ItemsName
                            });

                        }



                        else {
                            BOMLists.SciceParks.push({
                                ISPSIF_Id: d.ISPSIF_Id,
                                ISPSIF_Cost: d.ISPSIF_CostNew,
                                ISMSLET_Id: BOMLists.ISMSLET_Id,
                                ISPSIF_Quantity: ISPSIF_Quantity,
                                ISPSIF_ItemsName: d.ISMSPSI_ItemsName
                            });
                        }



                        //history multiple other than science park history
                        $scope.MtplotnScienceParkHistory.push({
                            ISMSBOM_Id: d.ISMSBOM_Id,
                            ISMSLE_Id: d.ISMSLE_Id,
                            ISPSIF_ItemsName: d.ISPSIF_ItemsName,
                            ISMSPSI_ItemsName: d.ISMSPSI_ItemsName,
                            ISMSPSI_ID: d.ISMSPSI_ID,
                            ISMSPSI_ItemFlag: d.ISMSPSI_ItemFlag,
                            ISMSPSI_ItemsDescription: d.ISMSPSI_ItemsDescription,
                            ISPSIF_Id: d.ISPSIF_Id,
                            ISPSIF_CostAppMd: d.ISPSIF_CostNew,
                            ISPSIF_CostOriginal: d.ISPSIF_Cost,
                            ISMSLET_Id: BOMLists.ISMSLET_Id,
                            ISPSIF_Quantity: d.ISPSIF_Quantity,
                            ISPSIF_Quantitys: d.ISPSIF_Quantitys,
                            ISMSBOM_Name: BOMLists.VMSMOUT_Name
                            //TotalCost: Math.round(Number(totalNewhistory) + (Number(d.ISPSIF_CostNew)))
                        });
                        if (BOMLists.ISMSLET_HtmlName == 'E-Learning' && BOMLists.VMSMOUT_Name == 'E-Learning') {
                            total = (Number(total) + (Number(d.ISPSIF_Costeach)));
                        } else {
                            total = (Number(total) + (Number(d.ISPSIF_CostNew)));
                        }


                        totalNewhistory = (Number(totalNewhistory) + (Number(d.ISPSIF_CostNew)));

                    });
                    //  

                    BOMLists.ISMSLET_CostApproved = Math.round(Number(total));

                    angular.forEach($scope.MtplotnScienceParkHistory, function (item) {
                        item.TotalCost = Math.round(Number(totalNewhistory));
                    });


                    // $scope.MtplotnScienceParkHistory.push({ TotalCost: Math.round(Number(totalNewhistory)) });
                    $scope.MtplotnScienceParkhistoryNew($scope.MtplotnScienceParkHistory);


                    if (BOMLists.ISMSLET_HtmlName == "Science Park Gold Package Outdoor" || BOMLists.ISMSLET_HtmlName == "Science Park Silver Package Outdoor" || BOMLists.ISMSLET_HtmlName == "Science Park Silver && Gold Package Outdoor") {
                        BOMLists.ScinceParkflag = $scope.obj.rdopunch;
                        if ($scope.selectedleadlist != null && $scope.selectedleadlist.length > 0) {
                            angular.forEach($scope.selectedleadlist, function (d) {
                                BOMLists.SciencePark_Boot.push({
                                    ISMLESPB_Flag: d.ItemName,
                                    ISMLESPB_First_Year: d.FirstYear,
                                    ISMLESPB_Second_Year: d.SecondYear,
                                    ISMLESPB_Third_Year: d.ThirdYear,
                                    ISMLESPB_Fourth_Year: d.FourYear,
                                    ISMLESPB_Fifth_Year: d.FifthYear,
                                    ISMLESPB_FlagName: d.PakageType
                                });
                            });

                            //---------------
                            $scope.history();
                            //---------------
                        }
                        $scope.obj.rdopunch = "OUTRETED";
                    }
                    else if (BOMLists.ISMSLET_HtmlName == "Robotic Lab") {
                        BOMLists.ScinceParkflag = $scope.obj.rdoRobotics;
                        if ($scope.selectroboticlist != null && $scope.selectroboticlist.length > 0) {
                            angular.forEach($scope.selectroboticlist, function (d) {
                                BOMLists.SciencePark_Boot.push({
                                    ISMLESPB_Flag: d.ItemName,
                                    ISMLESPB_First_Year: d.FirstYear,
                                    ISMLESPB_Second_Year: d.SecondYear,
                                    ISMLESPB_Third_Year: d.ThirdYear,
                                    ISMLESPB_Fourth_Year: d.FourYear,
                                    ISMLESPB_Fifth_Year: d.FifthYear,
                                    ISMLESPB_FlagName: d.PakageType
                                });


                            });
                        }
                        $scope.obj.rdoRobotics = "ROBOTICOUTRETED";
                    }
                    else {
                        BOMLists.ScinceParkflag = "";
                    }

                    $scope.selectedleadlist = []; $scope.selectroboticlist = [];
                    $('#pendingissues').modal('hide');
                    return;
                }

            });

        }

        $scope.updateYears = function (dd) {

            if (dd.SecondYear) {
                // Set the value of the other years to match SecondYear
                dd.ThirdYear = dd.SecondYear;
                dd.FourYear = dd.SecondYear;
                dd.FifthYear = dd.SecondYear;
            } else {
                // If SecondYear is empty or null, clear the other years
                dd.ThirdYear = '';
                dd.FourYear = '';
                dd.FifthYear = '';
            }
        };
        $scope.AmountCalculate = function () {
            var totalvaluesNew = 0;
            angular.forEach($scope.perstudentDetails, function (d) {
                totalvaluesNew = (Number(totalvaluesNew) + (Number(d.ISPSIF_CostNew)));
            });

            $scope.totalvalues = Math.round(totalvaluesNew);
        }
        $scope.AmountRobotic = function () {
            var totalvaluesNew = 0;
            angular.forEach($scope.perstudentDetails, function (d) {
                totalvaluesNew = (Number(totalvaluesNew) + (Number(d.ISPSIF_CostNew)));
            });

            $scope.totalvalues = Math.round(totalvaluesNew);
        }
        //perstudentDetails
        $scope.OnChangeBOMList = function (edits) {
            // $scope.bomList = [];
            $scope.WhatsValue = ""; $scope.obj.empName = ""; var WhatsValues = 0;
            var ISMSLET_Id = 0;
            // added new things
            var web1 = 0;
            var web2 = 0;
            var web3 = 0;
            $scope.web1Values = 0;
            $scope.web2Values = 0;
            $scope.web3Values = 0;


            $scope.ModuleName = []; $scope.paymentterms = []; $scope.groupedData = [];

            var BOMLists = []; $scope.selectedleadlist = []; $scope.addres1 = ""; $scope.addres2 = "";
           
           //BOMLists.push($scope.ISMSBOM_IdNew);

            if (edits != null && edits != "") {
                BOMLists.push(edits.PEP_ISMSBOM_Id);
                ISMSLET_Id = edits.PEP_Id;
                $scope.ISMSBOM_BOMNameCSM = edits.ISMSBOM_BOMName;
                $scope.Dept.ISMSLE_Id = edits.PEP_ISMSLE_ID;

                $scope.ApprovedstatusNew = edits.PEP_Status;
                $scope.showmodel = true;
            }
            var data = {
                "BOMListt": BOMLists,
                "ISMSLE_Id": $scope.Dept.ISMSLE_Id,
                "ISMSLET_Id": ISMSLET_Id,
                //"ISMSLBOM_BOMFlg": $scope.ISMSBOM_BOMNameCSM
                "ISMSLBOM_BOMFlg": "Proposal Customization"

            };










            //var data = {
            //    "BOMListt": BOMLists,
            //    "ISMSLE_Id": $scope.ISMSLET_IdNew,
            //    "ISMSLBOM_BOMFlg": 'Proposal Customization'
            //};
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeBOMList", data).then(function (promise) {
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

                    $scope.Moudetails = promise.perstudent_QutationReq;
                    $scope.ProductChanges = promise.paymentterms;
                    $scope.ClientChanges = promise.selectedleadlist;

                    if (($scope.ProductChanges != null && $scope.ProductChanges != undefined && $scope.ProductChanges.length > 0)
                        || ($scope.ClientChanges != null && $scope.ClientChanges != undefined && $scope.ClientChanges.length > 0) && $scope.showmodel==true)
                    {
                        $('#SwapModel').modal('show');
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

                        $scope.ISMSLET_Status = "";
                    }

                    //$('#SwapModel').modal('show');

                    //if ($scope.ClientChanges != undefined && $scope.ClientChanges != null && $scope.ClientChanges.length > 0) {

                       
                    //}

                   




                } else {
                    swal("No Records Found");
                }


            });
        };

        //        $scope.Aicamercalction = function (dd) {
        //            dd.ISPSIF_CostNew = (Number(dd.ISPSIF_Quantity) * (Number(dd.ISPSIF_Costeach)));
        //        }
        $scope.Aicamercalction = function (dd) {
            if (dd.ISPSIF_Quantitys > dd.ISPSIF_Quantity) {
                swal("Entered Quantity Should be Morethan the Minimum Quantity.");
                dd.ISPSIF_Quantity = dd.ISPSIF_Quantitys;
                return;
            }
            else if (dd.ISPSIF_Cost > dd.ISPSIF_Costeach) {
                swal("Entered Amount Should be Morethan the Minimum Amount.");
                dd.ISPSIF_Costeach = dd.ISPSIF_Cost;
                return;
            }
            else {
                dd.ISPSIF_CostNew = (Number(dd.ISPSIF_Quantity) * (Number(dd.ISPSIF_Costeach)));
                $scope.aicameratotalamountnew();
            }


        }


        $scope.calculateStdCost = function (dd) {
            var BOMLists = []; BOMLists.push(dd.ISMSBOM_Id);
            $scope.SCINCE_Parks = [];
            //perstudentDetails
            if ($scope.perstudentDetails != null && $scope.perstudentDetails.length > 0) {
                angular.forEach($scope.perstudentDetails, function (ddd) {
                    $scope.SCINCE_Parks.push({
                        ISPSIF_Id: ddd.ISPSIF_Id,
                        ISPSIF_Quantity: ddd.ISPSIF_Quantitys,
                        ISPSIF_Cost: ddd.ISPSIF_CostNew
                    })
                });
            }

            var data = {
                "SCINCE_Parks": $scope.SCINCE_Parks,
                "ISMSLE_Id": dd.ISMSLE_Id,
                "BOMListt": BOMLists,
                "Negotiation_Amount": dd.FirstYear

            }
            apiService.create("Lead_Target_Mapping/deactivate", data).then(function (promise) {
                if (promise.selectedleadlist != null && promise.selectedleadlist.length > 0) {
                    $scope.selectedleadlist = [];
                    $scope.selectedleadlist = promise.selectedleadlist;
                }
                if (promise.perstudent_QutationReq != null && promise.perstudent_QutationReq.length > 0) {
                    $scope.perstudentDetails = [];
                    $scope.perstudentDetails = promise.perstudent_QutationReq;
                }
            });
        }
        //
        $scope.calculatedInd = function (dd) {
            if (dd.IMSV_TotalCostPerQty > 0) {
                if ((Number(dd.IMSV_TotalCostPerQty)) > (Number(dd.ISPSIF_AmountScincePark))) {
                    swal("The entered cost should be greater than the vendor cost!");
                    dd.ISPSIF_AmountScincePark = (Math.round(Number(dd.ISPSIF_Cost) / (Number(dd.ISPSIF_Quantity))));
                    return;
                }
            }

            $scope.calculatedFormulas(dd);

        }
        $scope.calculatedRobo = function (dd) {
            if (dd.IMSV_TotalCostPerQty > 0) {
                if ((Number(dd.IMSV_TotalCostPerQty)) > (Number(dd.ISPSIF_AmountScincePark))) {
                    swal("The entered cost should be greater than the vendor cost!");
                    dd.ISPSIF_AmountScincePark = (Math.round(Number(dd.ISPSIF_Cost) / (Number(dd.ISPSIF_Quantity))));
                    return;
                }
            }

            $scope.calculatedRobotic(dd);

        }
        //calculatedRobotic
        $scope.calculatedRobotic = function (dd) {
            dd.ISPSIF_CostNew = (Math.round((Number(dd.ISPSIF_AmountScincePark) * (Number(dd.ISPSIF_Quantitys)))));
            $scope.AmountRobotic(dd); $scope.SCINCE_Parks = [];
            var BOMLists = [];
            $scope.SCINCE_Parks.push({
                ISPSIF_Id: dd.ISPSIF_Id,
                ISPSIF_Quantity: dd.ISPSIF_Quantitys,
                ISPSIF_Cost: dd.ISPSIF_CostNew
            })
            BOMLists.push(dd.ISMSBOM_Id);
            var data = {
                //  "SCINCE_Parks": $scope.SCINCE_Parks,
                "Robotic_Labs": $scope.SCINCE_Parks,
                "ISMSLE_Id": dd.ISMSLE_Id,
                "BOMListt": BOMLists

            }
            apiService.create("Lead_Target_Mapping/Saveboomlistpackage", data).then(function (promise) {
                if (promise.selectedleadlist != null && promise.selectedleadlist.length > 0) {
                    $scope.selectroboticlist = []; $scope.selectednew = [];
                    $scope.selectroboticlist = promise.selectedleadlist;
                    angular.forEach($scope.selectroboticlist, function (dd) {
                        dd.FirstYearNew = dd.FirstYear;
                        dd.SecondYearNew = dd.SecondYear;
                        dd.ThirdYearNew = dd.ThirdYear;
                        dd.FourYearNew = dd.FourYear;
                        dd.FifthYearNew = dd.FifthYear;
                    });

                    //$scope.selectednew = promise.selectedleadlist;
                }


            });

            //}

        }

        $scope.calculatedFormulas = function (dd, index) {
            if (index === 0 && $scope.perstudentDetails.length > 1) {
                var secondRow = $scope.perstudentDetails[1];
                if (secondRow) {
                    secondRow.ISPSIF_Quantitys = dd.ISPSIF_Quantitys;
                    secondRow.ISPSIF_CostNew = (Number(dd.ISPSIF_Quantitys) * (secondRow.ISPSIF_AmountScincePark));
                }

            }
            if (index === 3 && $scope.perstudentDetails.length > 1 && $scope.ismsboM_BOMName == 'Science Park Silver && Gold Package Outdoor') {
                var fifthRow = $scope.perstudentDetails[4];
                if (fifthRow) {
                    fifthRow.ISPSIF_Quantitys = dd.ISPSIF_Quantitys;
                    fifthRow.ISPSIF_CostNew = (Number(dd.ISPSIF_Quantitys) * (fifthRow.ISPSIF_AmountScincePark));

                }

            }
            dd.ISPSIF_CostNew = (Math.round((Number(dd.ISPSIF_AmountScincePark) * (Number(dd.ISPSIF_Quantitys)))));
            $scope.AmountCalculate(dd); $scope.SCINCE_Parks = [];
            var BOMLists = [];
            //                                                                                                                        
            $scope.SCINCE_Parks.push({
                ISPSIF_Id: dd.ISPSIF_Id,
                ISPSIF_Quantity: dd.ISPSIF_Quantitys,
                ISPSIF_Cost: dd.ISPSIF_CostNew
            })

            BOMLists.push(dd.ISMSBOM_Id);
            var data = {
                "SCINCE_Parks": $scope.SCINCE_Parks,
                "ISMSLE_Id": dd.ISMSLE_Id,
                "BOMListt": BOMLists

            }
            apiService.create("Lead_Target_Mapping/Saveboomlistpackage", data).then(function (promise) {
                if (promise.selectedleadlist != null && promise.selectedleadlist.length > 0) {
                    $scope.selectedleadlist = [];
                    $scope.selectedleadlist = promise.selectedleadlist;
                    angular.forEach($scope.selectedleadlist, function (dd) {
                        dd.FirstYearNew = dd.FirstYear;
                        dd.SecondYearNew = dd.SecondYear;
                        dd.ThirdYearNew = dd.ThirdYear;
                        dd.FourYearNew = dd.FourYear;
                        dd.FifthYearNew = dd.FifthYear;
                    });

                    $scope.selectednew = promise.selectedleadlist;
                }


            });

            //---------------
            $scope.history();
            //---------------

            //}

        }
        //


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

        $scope.OnachageDetails = function (temp) {
            var Totalstudent = 0; var TotalCost = 0;
            if (temp.VMSMOUT_Flag == 'M') {
                if (temp.ISMSLET_CostApproved > 0) {

                }
                else {
                    temp.ISMSLET_CostApproved = temp.ISMSLET_TotalNew;
                }
                TotalCost = (Number(temp.ISMSLET_CostApproved) / (Number(temp.AbvStrength)));

                Totalstudent = TotalCost / 12;
                temp.ISMSLET_Amount_IMD = Math.round(Number(Totalstudent));

            }


        }
        $scope.updateYearsRobotic = function (dd) {
            if (dd.FirstYear) {
                dd.SecondYear = dd.FirstYear;
                dd.ThirdYear = dd.FirstYear;
                dd.FourYear = dd.FirstYear;
                dd.FifthYear = dd.FirstYear;
            } else {

                dd.SecondYear = '';
                dd.ThirdYear = '';
                dd.FourYear = '';
                dd.FifthYear = '';
            }
        };
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
            var data = {
                "ISMSLE_Id": $scope.Dept.ISMSLE_Id,
                "bomListdetails": $scope.serviceitamlist
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


        $scope.SendMail = function () {
            var mgs = $scope.ismslE_EmailId;
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
                        var Template = document.getElementById("printSectionIdmails").innerHTML;
                        var Templates = document.getElementById("printSectionIdmailss").innerHTML;
                        var LeadIntimate = document.getElementById("LeadIntimate").innerHTML;
                        $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

                        var data = {
                            "ManagerMailid": $scope.ismslE_EmailId,
                            "ISMSLE_Id": $scope.ismslE_Id,
                            Template: Template,
                            Templates: Templates,
                            "ISMSLET_HtmlName": $scope.ismsboM_BOMName,
                            ISMSLET_HtmlTemplate: LeadIntimate,
                            "retrunwhatsup": "",
                            "ISMSLED_MobileNo": $scope.ismslE_ContactNo
                            //  "ISMSLED_MobileNo":9731690432
                        };
                        apiService.create("Lead_Target_Mapping/sendmailDetails", data).
                            then(function (promise) {
                                swal('Email  Sent Successfully.');
                                $state.reload();
                            });
                    }

                });
        }

        $scope.edit = function (edits) {
            $scope.bomList = [];
            $scope.VMSMOUT_Flag = "";
            //$scope.Dept.ISMSLE_Id = edits.ISMSLE_Id;
            $scope.Dept.ISMSLET_Id = edits.PEP_ISMSLE_ID;
            $scope.ISMSLET_IdNew = edits.PEP_ISMSLE_ID;
            $scope.ISMSBOM_IdNew = edits.PEP_ISMSBOM_Id;


            //$scope.ismsboM_BOMName = edits.ISMSLET_HtmlName;
            //$scope.splitValues = edits.ISMSBOM_Id;
            //$scope.bomListTemp = [];
            //$scope.VMSMOUT_Flag = edits.VMSMOUT_Flag;

            //$scope.bomListTemp.push({
            //    ISMSBOM_List: $scope.splitValues.split('#')
            //})
            //if ($scope.bomListTemp != null && $scope.bomListTemp.length > 0) {
            //    angular.forEach($scope.bomListTemp, function (dd) {
            //        angular.forEach(dd.ISMSBOM_List, function (d) {
            //            $scope.bomList.push({
            //                ISMSBOM_Id: d,
            //                selected: true
            //            })
            //        });
            //    });
            //}
            ////ISMSBOM_Id //selected

            //$scope.OnChangeBOMList();

        }
        $scope.editDemo = function (lead) {

            var data = {
                "ISMSLE_Id": lead.ISMSLE_Id,

            }
            apiService.create("Lead_Target_Mapping/Saveboomlistpackage", data).then(function (promise) {
                if (promise.productlist != null && promise.productlist.length > 0) {
                    $scope.get_deviationreport = promise.productlist;
                    $scope.employeeid = [];
                    angular.forEach($scope.get_deviationreport, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    });
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_deviationreport, function (dd) {
                            if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {

                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;

                    });
                    $('#editDemoN').modal('show');

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
            $scope.updateflag = true;
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
            $scope.ismsboM_BOMName = temp.ISMSLET_HtmlName;
            var e1 = angular.element(document.getElementById("LeadIntimate"));
            $compile(e1.html(temp.ISMSLET_HtmlTemplate))(($scope));
            $('#myModal1').modal('show');
        }


        $scope.SendPro = function (aa) {


            

            var Template = document.getElementById("printSectionIdmails").innerHTML;
            var LeadIntimate = document.getElementById("LeadIntimate").innerHTML;
            var data = {
                "ISMSLET_Id": $scope.Dept.ISMSLET_Id,
                "ISMSLET_HtmlName": $scope.ismsboM_BOMName,
                "ISMSLET_ApprovedRemark": aa.ISMSLET_ApprovedRemark,
                ISMSLET_HtmlTemplate: LeadIntimate,
                Template: Template
            };
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

            //apiService.create("Lead_Target_Mapping/SendMdApproval", data).then(function (promise) {
            //    if (promise.retrunMsg == "Add") {
            //        swal('Record Saved Successfully... !');
            //    }
            //    else {
            //        swal('Failed !');

            //    }
            //    $state.reload();
            //});
        };



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.leadTempDetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.selleadTempDetail.push(itm);
                }
                else {
                    $scope.selleadTempDetail.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.leadTempDetails.every(function (itm) { return itm.selected; });
            if ($scope.selleadTempDetail.indexOf(SelectedStudentRecord) === -1) {
                $scope.selleadTempDetail.push(SelectedStudentRecord);
            }
            else {
                $scope.selleadTempDetail.splice($scope.selleadTempDetail.indexOf(SelectedStudentRecord), 1);
            }
            if (SelectedStudentRecord.selected==true) {

                $scope.edit(SelectedStudentRecord);
            }
        }

        $scope.SendP = function () {
            $scope.SendProcees = [];
            if ($scope.selleadTempDetail != null && $scope.selleadTempDetail.length > 0) {

                angular.forEach($scope.selleadTempDetail, function (itm) {
                    if (itm.selected == true) {
                        $scope.SendProcees.push({
                            ISMSLET_ApprovedRemark: itm.PEP_Remarks,
                            ISMSLET_Amount_IMD: itm.ISMSLET_Amount_IMD,
                            ISMSLET_Amount_FinalMD: itm.PEP_tot_rate_PerManDay,
                            //flag: "ProposalCustomization",
                            ISMSLET_CostApproved: itm.PEP_tot_rate_PerManDay,
                            ISMSLET_Id: itm.PEP_Id,
                            ISMSLBOM_BOMName: itm.PEP_Status
                        })
                    }
                   

                });


                //var data = {
                //    //"ISMSLET_Id": $scope.obj.ISMSLET_Id,
                //    "ISMSLE_Id": $scope.ismslE_Id,
                //    "flag": "ProposalCustomization",
                //    ISMSLET_HtmlTemplate: LeadIntimate,
                //    Template: Template,
                //    "product": products,
                //    "BOMListt": BOMLists,
                //    "ISMSLBOM_BOMDescription": $scope.fmscoR_RefernceNo,
                //    //"preAmount_I": preAmount_I,
                //    //"PreAmount_Final": PreAmount_Final,
                //    //"bomListdetails": $scope.ChangesDetails,

                //    "ISMSLE_StudentStrength": $scope.ISMSLE_StudentStrength
                //};


                if ($scope.SendProcees != null && $scope.SendProcees.length > 0) {
                    var data = {
                        "approvTempList": $scope.SendProcees,
                        "flag": "ProposalCustomization",

                    };
                    apiService.create("Lead_Target_Mapping/MdApproval", data).then(function (promise) {
                        if (promise.retrunMsg == "Add") {
                            swal('Record Saved Successfully... !');
                        }
                        else {
                            swal('Failed !');
                        }
                        $state.reload();
                    });
                }
                else {
                    swal('Please Save the Project Cost To Proceed...!');
                    //swal('Please Select At Least One Check Box  !');
                }

            }
            else {
                swal('Please Select At Least One Check Box  !');
            }

        };


        //---------------history ---------------//
        $scope.history = function () {
            $scope.goldandsilver = $scope.perstudentDetails;
            $scope.goldandsilverNew = [];
            angular.forEach($scope.perstudentDetails, function (dd) {
                $scope.goldandsilverNew.push({
                    IMSV_TotalCostPerQty: dd.IMSV_TotalCostPerQty,
                    ISMSBOM_Id: dd.ISMSBOM_Id,
                    ISMSLE_Id: dd.ISMSLE_Id,
                    ISMSPSI_ItemFlag: dd.ISMSPSI_ItemFlag,
                    ISMSPSI_ItemsDescription: dd.ISMSPSI_ItemsDescription,
                    ISPSIF_AmountScincePark: dd.ISPSIF_AmountScincePark,
                    ISPSIF_AmtScincePark: dd.ISPSIF_AmtScincePark,
                    ISPSIF_Cost: dd.ISPSIF_Cost,
                    ISPSIF_CostNew: dd.ISPSIF_CostNew,
                    ISPSIF_Id: dd.ISPSIF_CostNew,
                    ISPSIF_ItemsName: dd.ISPSIF_ItemsName,
                    ISPSIF_Quantity: dd.ISPSIF_Quantity,
                    ISPSIF_Quantitys: dd.ISPSIF_Quantitys,
                    ScienceParkFlag: $scope.obj.rdopunch
                })
            });
            $scope.goldandsilverYrwise = $scope.selectedleadlist;
            $scope.goldandsilverYrwiseNew = [];
            angular.forEach($scope.selectedleadlist, function (ddd) {
                $scope.goldandsilverYrwiseNew.push({
                    Cost: ddd.Cost,
                    FifthYear: ddd.FifthYear,
                    FifthYearNew: ddd.FifthYearNew,
                    FourYear: ddd.FourYear,
                    FourYearNew: ddd.FourYearNew,
                    ThirdYear: ddd.ThirdYear,
                    ThirdYearNew: ddd.ThirdYearNew,
                    SecondYear: ddd.SecondYear,
                    SecondYearNew: ddd.SecondYearNew,
                    FirstYear: ddd.FirstYear,
                    FirstYearNew: ddd.FirstYearNew,
                    ISMSBOM_Id: ddd.ISMSBOM_Id,
                    ISMSLE_Id: ddd.ISMSLE_Id,
                    ItemName: ddd.ItemName,
                    PakageType: ddd.PakageType,
                    ScienceParkFlag: $scope.obj.rdopunch

                })
            });


            var data = {
                "goldandsilverData": $scope.goldandsilverNew,
                "goldandsilverYrwiseData": $scope.goldandsilverYrwiseNew
            }
            apiService.create("Lead_Target_Mapping/SaveScienceParkTransaction", data).then(function (promise) {
                if (promise.retrunMsg == "Add") {
                    //swal('Record Saved Successfully... !');
                }
                else {
                    //swal('Failed !');

                }
            });

        }
        //---------------history ---------------//





        //added new things


        $scope.MtplotnScienceParkhistoryNew = function (datanew) {
            var data = {
                "ISMSalesMultipleTransaction": datanew
            }
            apiService.create("Lead_Target_Mapping/SaveISMSalesMultipleTransaction", data).then(function (promise) {
                if (promise.retrunMsg == "Add") {
                    //swal('Record Saved Successfully... !');
                }
                else {
                    //swal('Failed !');

                }
            });
        }



        //e learing
        $scope.AmountElearning = function () {
            var totalvaluesNew = 0;
            angular.forEach($scope.perstudentDetails, function (d) {
                totalvaluesNew = (Number(totalvaluesNew) + (Number(d.ISPSIF_CostNew)));
            });

            $scope.totalvalues = Math.round(totalvaluesNew);
        }

        $scope.calculatedIndElearning = function (dd) {
            var newQuantity = dd.ISPSIF_Quantity;
            var stopLoop = false;
            angular.forEach($scope.perstudentDetails, function (itemnew) {
                if (stopLoop) return;
                var eachqtycost = (Math.round(Number(itemnew.ISPSIF_Cost) / (Number(itemnew.ISPSIF_Quantity))));
                var totcostnew = (Number(eachqtycost) * Number(itemnew.ISPSIF_Quantity));
                var minqty = itemnew.ISPSIF_Quantitys;
                var newqty = itemnew.ISPSIF_Quantity;
                // Now compare with eLearning details
                angular.forEach($scope.perstudentDetailsElearning, function (aa) {

                    if (aa.ISPSIF_Id == itemnew.ISPSIF_Id) {
                        if (minqty > newqty) {

                            swal("The entered Classes should be greater than the minimum Classes!");
                            itemnew.ISPSIF_Quantity = itemnew.ISPSIF_Quantitys;
                            angular.forEach($scope.perstudentDetails, function (item11) {
                                if (item11.ISMSPSI_ItemFlag != 'B' || item11.ISPSIF_ItemsName != 'Maintenance') {
                                    item11.ISPSIF_Quantity = aa.ISPSIF_Quantitys;
                                }


                            });
                            stopLoop = true;
                            return;
                        }
                        else if (minqty < newqty) {

                            //angular.forEach($scope.perstudentDetails, function (item) {
                            //    if (item.ISMSPSI_ItemFlag != 'B' || item.ISPSIF_ItemsName != 'Maintenance  4 years') {
                            //        item.ISPSIF_Cost = (Number(eachqtycost) * Number(newqty));
                            //        item.ISPSIF_Costeach = (Number(eachqtycost) * Number(newqty));
                            //    }
                            //});

                            // if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance  4 years') {
                            itemnew.ISPSIF_Cost = Number(eachqtycost) * Number(newqty);
                            itemnew.ISPSIF_Costeach = Number(eachqtycost) * Number(newqty);
                            //}
                            stopLoop = true;
                            return;

                        }
                        else if (Number(aa.ISPSIF_CostNew) > Number(totcostnew)) {
                            itemnew.ISPSIF_Cost = itemnew.ISPSIF_Costeach;


                            angular.forEach($scope.perstudentDetails, function (item) {
                                //if (item.ISMSPSI_ItemFlag != 'B' || item.ISPSIF_ItemsName != 'Maintenance  4 years') {
                                item.ISPSIF_Quantity = aa.ISPSIF_Quantitys;
                                //}
                            });
                            swal("The entered cost should be greater than the vendor cost!");

                            stopLoop = true;
                            return;
                        }
                    }
                });

            });

            $scope.ElearningTotalCost();//calculation total amount
            return;


        }
        $scope.yearchangeelearning = function (years) {
            angular.forEach($scope.perstudentDetails, function (d) {
                if (d.ISMSPSI_ItemFlag == 'B') {
                    d.ISPSIF_Quantity = Number(years) - 1;
                    d.ISPSIF_Costeach = Number(d.ISPSIF_Cost) * Number(d.ISPSIF_Quantity);
                }
                else if (d.ISPSIF_ItemsName == 'Anti Virus' || d.ISPSIF_ItemsName == 'Digital Content') {
                    d.ISPSIF_Costeach = Number(d.ISPSIF_Cost) * Number(d.ISPSIF_Quantity) * Number(years);
                }

            });

            $scope.ElearningTotalCost();
        }

        $scope.calculatedIndElearningQty = function (dd, aa, years) {
            var newQuantity = dd.ISPSIF_Quantity;
            var minqty = dd.ISPSIF_Quantitys;

            if (minqty > newQuantity) {
                swal("The entered Quantity should be greater than the minimum Classes!");
                angular.forEach($scope.perstudentDetails, function (itemnew) {
                    if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance') {
                        if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                            itemnew.ISPSIF_Quantity = dd.ISPSIF_Quantitys;
                        } else {
                            itemnew.ISPSIF_Quantity = dd.ISPSIF_Quantitys;
                        }

                    }
                    //else if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                    //    itemnew.ISPSIF_Quantity = dd.ISPSIF_Quantitys;
                    //}
                });

                angular.forEach($scope.perstudentDetails, function (itemnew) {
                    //if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance  4 years') {
                    //    itemnew.ISPSIF_Cost = itemnew.ISPSIF_CostNew;
                    //    itemnew.ISPSIF_Costeach = itemnew.ISPSIF_CostNew;

                    //}

                    if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance') {
                        if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                            itemnew.ISPSIF_Cost = itemnew.ISPSIF_CostNew;
                            itemnew.ISPSIF_Costeach = itemnew.ISPSIF_Cost * itemnew.ISPSIF_Quantitys * Number(years);
                        } else {
                            itemnew.ISPSIF_Cost = itemnew.ISPSIF_CostNew;
                            itemnew.ISPSIF_Costeach = itemnew.ISPSIF_Costeach;
                        }


                    }
                    //else if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                    //    itemnew.ISPSIF_Cost = itemnew.ISPSIF_CostNew;
                    //    itemnew.ISPSIF_Costeach = itemnew.ISPSIF_Costeach * Number(years);
                    //}

                });

                return;
            } else {

                angular.forEach($scope.perstudentDetails, function (itemnew) {
                    if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance') {
                        if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                            itemnew.ISPSIF_Quantity = newQuantity;
                        } else {
                            itemnew.ISPSIF_Quantity = newQuantity;
                        }

                    }
                    //else if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                    //    itemnew.ISPSIF_Costeach = newQuantity * Number($scope.years);
                    //}
                });



                //below code for entire amount

                //angular.forEach($scope.perstudentDetails, function (itemnew) {
                //    var quantity = Number(itemnew.ISPSIF_Quantity);
                //    var eachqtycost = Math.round(Number(itemnew.ISPSIF_Cost) / Number(itemnew.ISPSIF_Quantitys));

                //    var totcostnew = (Number(eachqtycost) * Number(quantity));
                //    var minqty = itemnew.ISPSIF_Quantitys;
                //    var newqty = itemnew.ISPSIF_Quantity;

                //    if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance  4 years') {
                //        itemnew.ISPSIF_Cost = totcostnew;
                //        itemnew.ISPSIF_Costeach = totcostnew;
                //    }
                //});



                //below code for qty * cost per ech qty

                angular.forEach($scope.perstudentDetails, function (itemnew) {


                    if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance') {
                        if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                            itemnew.ISPSIF_Costeach = Number(itemnew.ISPSIF_Cost) * Number(itemnew.ISPSIF_Quantity) * Number(years);
                        } else {
                            itemnew.ISPSIF_Costeach = Number(itemnew.ISPSIF_Quantity) * Number(itemnew.ISPSIF_Cost);

                        }



                    }
                    //else if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                    //    itemnew.ISPSIF_Costeach = Number(d.ISPSIF_Cost) * Number(itemnew.ISPSIF_Quantity) * Number($scope.years);
                    //}
                });




            }






            $scope.ElearningTotalCost();//calculation total amount
            return;


        }



        $scope.calculatedIndElearningCost = function (dd, aa, years) {
            var oldcost = dd.ISPSIF_CostNew;
            var newcost = dd.ISPSIF_Cost;

            if (dd.ISPSIF_CostNew > dd.ISPSIF_Cost) {
                swal("The entered cost should be greater than the minimum cost!");
                if (dd.ISPSIF_ItemsName == 'Anti Virus' || dd.ISPSIF_ItemsName == 'Digital Content') {
                    dd.ISPSIF_Cost = dd.ISPSIF_CostNew;
                    dd.ISPSIF_Costeach = dd.ISPSIF_Cost * dd.ISPSIF_Quantity * Number(years);
                } else {
                    dd.ISPSIF_Cost = dd.ISPSIF_CostNew;
                }
                // dd.ISPSIF_Cost = dd.ISPSIF_CostNew;
                return;
            }
            else {

                angular.forEach($scope.perstudentDetails, function (itemnew) {
                    if (itemnew.ISMSPSI_ID == dd.ISMSPSI_ID) {
                        if (itemnew.ISMSPSI_ItemFlag != 'B' || itemnew.ISPSIF_ItemsName != 'Maintenance') {
                            itemnew.ISPSIF_Cost = dd.ISPSIF_Cost;
                            itemnew.ISPSIF_Costeach = Number(itemnew.ISPSIF_Quantity) * Number(itemnew.ISPSIF_Cost);
                        }
                        else if (itemnew.ISPSIF_ItemsName == 'Anti Virus' || itemnew.ISPSIF_ItemsName == 'Digital Content') {
                            itemnew.ISPSIF_Costeach = Number(itemnew.ISPSIF_Cost) * Number(itemnew.ISPSIF_Quantity) * Number(years);
                        }


                    }

                });





            }

 

            $scope.ElearningTotalCost();//calculation total amount
            return;


        }




        $scope.ElearningTotalCost = function () {
            var totalvaluesNew = 0;
            angular.forEach($scope.perstudentDetails, function (d) {
                totalvaluesNew = (Number(totalvaluesNew) + (Number(d.ISPSIF_Costeach)));
            });

            $scope.totalvalues11 = Math.round(totalvaluesNew);
            console.log("Total value", $scope.totalvalues11)
            //return;
        }



        //$scope.synchronizeQuantity = function (changedItem) {
        //    var newQuantity = changedItem.ISPSIF_Quantity;

        //    // Update all items with the new quantity
        //    angular.forEach($scope.perstudentDetails, function (item) {
        //        item.ISPSIF_Quantity = newQuantity;  // Synchronize quantity
        //       // item.ISPSIF_CostNew = (parseFloat(item.ISPSIF_Cost) || 0) * newQuantity; // Update cost based on new quantity
        //    });
        //   // $scope.calculatedIndElearning(changedItem);

        //};



        $scope.aicameratotalamountnew = function () {
            var totalaicamera = 0;
            angular.forEach($scope.perstudentDetails, function (d) {
                totalaicamera = (Number(totalaicamera) + (Number(d.ISPSIF_CostNew)));
            });
            $scope.totalvaluesaicamera = totalaicamera;
        }

        $scope.view = function (aa) {

            $scope.showmodel = true;
           
           
            $scope.OnChangeBOMListView(aa);
            //if (($scope.ProductChanges != null && $scope.ProductChanges != undefined && $scope.ProductChanges.length > 0)
            //    || ($scope.ClientChanges != null && $scope.ClientChanges != undefined && $scope.ClientChanges.length > 0)) {
            //    $('#SwapModel').modal('show');
            //}


        }
        $scope.OnChangeBOMListView = function (edits) {
            // $scope.bomList = [];
            $scope.WhatsValue = ""; $scope.obj.empName = ""; var WhatsValues = 0;
            var ISMSLET_Id = 0;
            // added new things
            var web1 = 0;
            var web2 = 0;
            var web3 = 0;
            $scope.web1Values = 0;
            $scope.web2Values = 0;
            $scope.web3Values = 0;


            $scope.ModuleName = []; $scope.paymentterms = []; $scope.groupedData = [];

            var BOMLists = []; $scope.selectedleadlist = []; $scope.addres1 = ""; $scope.addres2 = "";
            $scope.Moudetails = [];
            $scope.ProductChanges = [];
            $scope.ClientChanges = [];
          
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
                if (promise.ismslboM_BOMFlg == "Proposal Customization") {
                    //if (promise.bomfinallist != null && promise.bomfinallist.length > 0) {
                    //    angular.forEach(promise.bomfinallist, function (d) {

                    //        if (d.VMSMOUT_Name == promise.ismslboM_BOMFlg) {
                    //            var e1 = angular.element(document.getElementById("LeadIntimate"));
                    //            $scope.xtra = d.VMSMOUT_Template;
                    //            e1.html($scope.xtra);
                    //            $compile(e1.contents())($scope);
                    //            $scope.obj.VMSMOUT_Flag = d.VMSMOUT_Flag;

                    //            return;
                    //        }
                    //    });



                    //}

                    $scope.Moudetails = promise.perstudent_QutationReq;
                    $scope.ProductChanges = promise.paymentterms;
                    $scope.ClientChanges = promise.selectedleadlist;

                    if (($scope.ProductChanges != null && $scope.ProductChanges != undefined && $scope.ProductChanges.length > 0)
                        || ($scope.ClientChanges != null && $scope.ClientChanges != undefined && $scope.ClientChanges.length > 0)) {




                        var productmanhour = 0;
                        var clientmanhour = 0;
                        var clientrateperday = 0;
                        var clienttotalamt = 0;


                
                        

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
                        $scope.totclienttotalamt = clienttotalamt;












                        $('#SwapModel').modal('show');
                    } else {
                        swal("No Details Found..");
                    }


                    //if (promise.bomTemplateList != undefined && promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                    //    $scope.bomTemplateList = promise.bomTemplateList;
                    //    $scope.ismslE_LeadName = $scope.bomTemplateList[0].ISMSLE_LeadName;
                    //    $scope.ismsmpR_ProductName = $scope.bomTemplateList[0].ISMSMPR_ProductName;
                    //    $scope.ismslE_ContactDesignation = $scope.bomTemplateList[0].ISMSLE_ContactDesignation;
                    //    $scope.addres = $scope.bomTemplateList[0].addres;
                    //    $scope.ismslE_ContactNo = $scope.bomTemplateList[0].ISMSLE_ContactNo;
                    //    $scope.ismslE_ContactPerson = $scope.bomTemplateList[0].ISMSLE_ContactPerson;
                    //    $scope.ismslE_EmailId = $scope.bomTemplateList[0].ISMSLE_EmailId;
                    //    $scope.fmscoR_RefernceNo = $scope.bomTemplateList[0].FMSCOR_RefernceNo;
                    //    $scope.hrmE_EmployeeFirstName = $scope.bomTemplateList[0].HRME_EmployeeFirstName;
                    //    $scope.ismspS_Service_Name = $scope.bomTemplateList[0].ISMSPS_Service_Name;
                    //    $scope.ismslE_Id = $scope.bomTemplateList[0].ISMSLE_Id;

                    //    $scope.ISMSLET_Status = "";
                    //}

                   






                } else {
                    swal("No Records Found");
                }


            });
        };






        $scope.Calculatetotamount = function (aaa) {
            var totalamountcalculated = 0;

           
            totalamountcalculated = Math.round(Number(aaa.PEP_Tot_ManDaySpent) * Number(aaa.PEP_tot_rate_PerManDay));
              aaa.PEP_tot_cost = Math.round(Number(totalamountcalculated));
                   

           
         
           
        }


    }

})();

