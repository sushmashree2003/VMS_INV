(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_Advance_TourController', TADA_Advance_TourController)
    TADA_Advance_TourController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', '$q', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADA_Advance_TourController($rootScope, $scope, $state, $location, Flash, $q, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
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
        $scope.obj = {};
        $scope.obj.returnval = false;
        $scope.IER_IDNew = "";
        $scope.searchthree = "";
        $scope.IER_ID = 0;
        $scope.searchValue = "";
        $scope.srresult = [];
        $scope.erresult = [];
        $scope.LeadListTemp = [];
        $scope.Datacategory = [];
        $scope.srresult = [];
        $scope.erresult = [];
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 5;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 5;
        $scope.currentPage4 = 1;
        $scope.searchtwo = "";
        $scope.coptyright = copty;
        $scope.itemsPerPage4 = 5;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //
        $scope.imagename = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
            $scope.imagename = logopath;
        }
        //  $scope.INVMSL_SalesDate = new Date();


        $scope.maxEndDate = new Date();
        $scope.maxEndDate.setDate($scope.maxEndDate.getDate() + 29);
        $scope.plMaxdate = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate())
        $scope.GetCategoryArray = [];
        $scope.GetMandatary = []
        $scope.GetCategoryTemp = [];
        $scope.PlanName = "";
        $scope.TourRemark = "";
        $scope.ismeridian = true;
        $scope.Priority = [];
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.hrmE_Id = "";
        $scope.empname = "";
        $scope.HRME_EmailId = "";
        $scope.statename = "";
        $scope.emailid = ""; $scope.get_Report = [];
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_Advance_Tour/getalldata", pageid).
                then(function (promise) {
                    // $scope.GetCategoryArray = promise.getCategoryArray;
                    angular.forEach(promise.getCategoryArray, function (emp) {
                        if (emp.IMRC_Flag == "BDM") {
                            $scope.GetMandatary.push(emp);
                            $scope.GetCategoryArray.push(emp);
                        }
                    });
                    $scope.GetCategoryTemp = promise.getCategoryArray;
                    if (promise.getPlanerdata != null && promise.getPlanerdata.length > 0) {

                        angular.forEach(promise.getPlanerdata, function (emp) {
                            if (emp.IERA_StatusFlg == "TLB") {
                                $scope.get_Report.push(emp);
                            }
                        });
                    }


                    $scope.getClient = promise.clientlist;
                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.emailid = promise.getCategoryArray[0].HRME_EmailId;
                    $scope.MI_Id = promise.mI_Id;
                    if (promise.tour_Details != null && promise.tour_Details.length > 0) {
                        $scope.Priority = promise.tour_Details;
                        $scope.stateliist = [];
                        angular.forEach($scope.Priority, function (dev) {
                            if ($scope.stateliist.length === 0) {
                                $scope.stateliist.push(dev);
                            }
                            else if ($scope.stateliist.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.stateliist, function (emp) {
                                    if (emp.IVRMMS_Id === dev.IVRMMS_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.stateliist.push(dev);
                                }
                            }
                        })

                        if ($scope.stateliist != null && $scope.stateliist.length > 0) {
                            $scope.statename = "";
                            angular.forEach($scope.stateliist, function (emp) {
                                if ($scope.statename != "") {
                                    $scope.statename = $scope.statename + "," + emp.IVRMMS_Name
                                }
                                else {
                                    $scope.statename = emp.IVRMMS_Name
                                }

                            });
                        }

                    }
                    if ($scope.GetCategoryArray != null && $scope.GetCategoryArray.length > 0) {
                        $scope.empname = $scope.GetCategoryArray[0].HRME_EmployeeFirstName;
                    }


                })
        }

        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.Datacategorysave = []; $scope.Closureminmax = [];
                $scope.Closurename = []; $scope.CategoryParameters = [];
                angular.forEach($scope.Datacategory, function (emp) {
                    if (emp.IMRC_CategoryName === 'Closure') {
                        $scope.Closurename.push(emp);
                    }
                });
                angular.forEach($scope.GetMandatary, function (emp) {
                    if (emp.IMRC_CategoryName == "Closure") {
                        $scope.Closureminmax.push(emp);
                     
                    }
                });
                

                if ($scope.Datacategory != null && $scope.Datacategory.length > 0) {

                    if ( $scope.Closurename.length >= $scope.Closureminmax[0].IMECM_MaxTarget) {
                        $scope.employeeid = [];
                        angular.forEach($scope.Datacategory, function (dev) {
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
                        if ($scope.GetCategoryArray.length == $scope.employeeid.length) {

                            angular.forEach($scope.Datacategory, function (dd) {
                                $scope.Datacategorysave.push({
                                    ClientId: dd.ISMSLE_Id,
                                    CategoryId: dd.IMRC_CategoryId,
                                    ISMSMPR_Id: dd.ISMSMPR_Id
                                })
                            })
                            var msg = document.getElementById("invoicemail").innerHTML;
                            var data = {
                                "PlanStartDate": new Date($scope.startdate).toDateString(),
                                "PlanEndDate": new Date($scope.enddate).toDateString(),
                                "listEmployeeSales": $scope.Datacategorysave,
                                "PlanName": $scope.PlanName,
                                "TourRemark": $scope.TourRemark,
                                "returnval": msg,
                                "IERA_StatusFlg": "TLB"
                                // "IERC_ID": $scope.IERC_ID

                            };

                            apiService.create("TADA_Advance_Tour/saveallowence", data).then(function (promise) {
                                if (promise.message == "save") {
                                    swal("Records Save Sucessfully !");
                                }
                                else if (promise.message == "notsave") {
                                    swal("Records Not Saved  !");
                                }
                                else if (promise.message == "admin") {
                                    swal("Please Contact Administrator !");
                                }
                                else {
                                    swal("Records Not Saved  !");
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("All category Not  Selected !");
                        }


                    }
                    else {
                        swal("Select maximum " + $scope.Closureminmax[0].IMECM_MaxTarget +
                            " Closure categories and at least complete " + $scope.Closureminmax[0].IMECM_MinTarget +
                            " Closure categories ..!");
                    }
                }
                else {
                    
                    swal("Category Not selected !")
                }




            }


        };
        $scope.Onchangecategory = function () {
            $scope.LeadList = [];
            if ($scope.IMRC_CategoryId > 0) {
                var data = {
                    "IMRC_CategoryId": $scope.IMRC_CategoryId
                };
                apiService.create("TADA_Advance_Tour/TadaAllwence", data).then(function (promise) {
                    if (promise.leadList != null && promise.leadList.length > 0) {
                        $scope.LeadList = promise.leadList;
                    }
                    else {
                        swal("Leads Are Not  Create !");
                    }

                });
            }



        };
        $scope.OnchangecategoryNew = function () {
            $scope.LeadList = [];
            if ($scope.IMRC_CategoryIdNew > 0) {
                var data = {
                    "IMRC_CategoryId": $scope.IMRC_CategoryIdNew
                };
                apiService.create("TADA_Advance_Tour/TadaAllwence", data).then(function (promise) {
                    $scope.LeadList = promise.leadList;

                });
            }



        };
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        $scope.addgrnrowsclient = function () {
            if ($scope.clientnames_rem.length > 1) {
                for (var x = 0; x === $scope.clientnames_rem.length; x++) {
                    var id1 = $scope.clientnames_rem[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.clientnames_rem.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };

        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.clientnames_rem.length - 1;
            $scope.clientnames_rem.splice(index, 1);

        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interactedtada = function (field) {
            return $scope.submitted1;
        };
        $scope.interacteddc = function (field) {
            return $scope.submitted2;
        };
        //interacted
        //interacted
        $scope.AddToCart = function () {
            $scope.IMRC_CategoryName = "";
            $scope.Datacategorytemp = [];

            if ($scope.GetCategoryArray != null && $scope.GetCategoryArray.length > 0 && $scope.IMRC_CategoryId > 0) {
                angular.forEach($scope.GetCategoryArray, function (dd) {
                    if (dd.IMRC_CategoryId == $scope.IMRC_CategoryId) {
                        $scope.IMRC_CategoryName = dd.IMRC_CategoryName;
                    }
                })
                //  $scope.LeadListTemp = $scope.LeadList;
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
                            //IMRC_CategoryId: $scope.IMRC_CategoryId,
                            ISMSMPR_Id: dd.ISMSMPR_Id,
                            IVRMMS_Id: dd.IVRMMS_Id
                        });
                        $scope.LeadListTemp.push(dd);

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
        //Deletedata
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {
                "IER_ID": item.IER_ID

            }
            var dystring = "";
            if (item.ISM_Active == true) {
                dystring = "Deactivate";
            }
            else if (item.ISM_Active == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADA_Advance_Tour/deactive", data).
                            then(function (promise) {
                                if (promise.message == "save") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.message == "notsave") {
                                    swal("Record  Not Deactivate");
                                }
                                else if (promise.message == "admin") {
                                    swal("Plaese Conact Administrtor !");
                                }


                                $state.reload();


                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                    $state.reload();
                });
        }
        //Editdata
        $scope.Editdata = function (item) {
            $scope.IER_ID = 0;
            $scope.plandate = ""; $scope.planenddate = "";
            $scope.LeadList = [];
            $scope.Datacategory = [];
            $scope.IER_ID = item.IER_ID;
            var data = {
                "IER_ID": item.IER_ID

            };
            apiService.create("TADA_Advance_Tour/EditData", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.startdate = new Date(promise.editdata[0].ieR_Plan_StartDate);
                    $scope.enddate = new Date(promise.editdata[0].ieR_Plan_EndDate);
                    $scope.TourRemark = promise.editdata[0].ieR_Remarks;
                    $scope.PlanName = promise.editdata[0].ieR_PlanName;
                    $scope.IER_ID = promise.editdata[0].ieR_ID;
                    $scope.Datacategory = promise.editdataOthelist;
                    $scope.plandate = new Date(promise.editdata[0].ieR_Plan_StartDate);
                    $scope.planenddate = new Date(promise.editdata[0].ieR_Plan_EndDate);
                }
                else {
                    $scope.IER_ID = 0;
                }
            });


        };
        //Update
        $scope.Update = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.Datacategorysave = [];
                if ($scope.Datacategory != null && $scope.Datacategory.length > 0) {
                    $scope.employeeid = [];
                    angular.forEach($scope.Datacategory, function (dev) {
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

                    angular.forEach($scope.Datacategory, function (dd) {
                        $scope.Datacategorysave.push({
                            ClientId: dd.ISMSLE_Id,
                            CategoryId: dd.IMRC_CategoryId,
                            ISMSMPR_Id: dd.ISMSMPR_Id
                        })
                    })
                    var data = {
                        "PlanStartDate": new Date($scope.startdate).toDateString(),
                        "PlanEndDate": new Date($scope.enddate).toDateString(),
                        "listEmployeeSales": $scope.Datacategorysave,
                        "PlanName": $scope.PlanName,
                        "TourRemark": $scope.TourRemark,
                        "IER_ID": $scope.IER_ID,
                        "IERA_StatusFlg": "TL"

                    };

                    apiService.create("TADA_Advance_Tour/updateallowence", data).then(function (promise) {
                        if (promise.message == "save") {
                            swal("Records Updated Sucessfully !");
                        }
                        else if (promise.message == "notsave") {
                            swal("Records Not Saved  !");
                        }
                        else if (promise.message == "admin") {
                            swal("Please Contact Administrator !");
                        }
                        else {
                            swal("Records Not Saved  !");
                        }
                        $state.reload();
                    });

                }
                else {
                    swal("Category Not slected !")
                }


            }


        };
        //Calculate
        $scope.CityDistance = [];
        $scope.RepeatDetailsNew = [];
        $scope.Calculate = function () {
            $scope.submitted1 = true;
            $scope.claclateTada = true; $scope.claclateTravel = true;
            if ($scope.myForm2.$valid) {
                $scope.RepeatDetailsTemp = [];
                if ($scope.RepeatDetails != null && $scope.RepeatDetails.length > 0) {
                    angular.forEach($scope.RepeatDetails, function (dd) {
                        $scope.srresult = [];
                        $scope.erresult = [];
                        var IRPD_DepartureTime = dd.IRPD_DepartureTime;
                        var IRPD_ArrivalTime = dd.IRPD_ArrivalTime;
                        $scope.sresult = $filter('date')(IRPD_DepartureTime, 'HH:mm');
                        $scope.eresult = $filter('date')(IRPD_ArrivalTime, 'HH:mm');
                        $scope.RepeatDetailsTemp.push({
                            IER_ID: dd.IER_ID,
                            IVRMMS_Id: dd.IVRMMS_Id,
                            IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate).toDateString(),
                            IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate).toDateString(),
                            IRPD_DepartureTime: $scope.sresult,
                            IRPD_ArrivalTime: $scope.eresult,
                            IRPD_Order: dd.IRPD_Order
                        })
                    })
                }
                var data = {
                    "Plan_DepartureDetails": $scope.RepeatDetailsTemp,
                    "IER_ID": $scope.IER_IDNew
                };
                apiService.create("TADA_Advance_Tour/CalculateTada", data).then(function (promise) {
                    if (promise.message == "save") {
                        swal("Record calculated Successfully  !");
                        $scope.Fromstate = promise.stateList;

                        //$scope.Tostate = promise.stateList;
                        $scope.Travel_data = promise.travel_data;
                        $scope.RepeatDetails = [];
                        angular.forEach(promise.editdataOthelist, function (dd) {
                            $scope.RepeatDetails.push({
                                IER_ID: dd.IER_ID,
                                IVRMMS_Id: dd.IVRMMS_Id,
                                IVRMMS_Name: dd.IVRMMS_Name,
                                IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                                IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                                IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                                IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                                IRPD_Order: dd.IRPD_Order
                            })

                        })

                        $scope.CategoryParameters = [];
                        if (promise.leadList != null && promise.leadList.length > 0) {
                            $scope.CategoryParameters = promise.leadList;
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

                        //
                        //Tour_Details
                        $scope.Tour_Details = [];
                        if (promise.tour_Details != null && promise.tour_Details.length > 0) {
                            $scope.Tour_Details = promise.tour_Details;
                            angular.forEach($scope.Tour_Details, function (dd) {
                                dd.chk_amt = false;
                                if (dd.Particular == "Other") {
                                    $scope.claclateTada = false;
                                }
                                if (dd.Particular == "Travel") {
                                    $scope.claclateTravel = false;
                                }
                            })
                        }
                        else {
                            swal("Ur Designation  Tour  Amount Is Not Maped !");
                            $scope.Tour_Details = [];
                            return;
                        }
                        if ($scope.claclateTravel == true) {
                            $scope.Tour_Details.push({
                                Particular: "Travel",
                                Amount: "",
                                TotalAmt: "",
                                RoutePlanDays: "",
                                Slots: "",
                                chk_amt: false
                            })
                        }
                        if ($scope.claclateTada == true) {
                            $scope.Tour_Details.push({
                                Particular: "Other",
                                Amount: "",
                                TotalAmt: "",
                                RoutePlanDays: "",
                                Slots: "",
                                chk_amt: false
                            })
                        }
                        //


                    }
                    else if (promise.message == "notsave") {
                        swal("Record  Not saved !");
                    }
                    else if (promise.message == "admin") {
                        swal("Plaese Conact Administrtor !");
                    }
                    //adeed
                    $scope.RepeatDetailsNew = $scope.RepeatDetails;
                });
            }
        }
        $scope.txt_approxTotAmount = 0;
        $scope.calcuateValidate = function (cla) {
            var taxpercentage = 0; cla.enableflag = true; var txt_approxTotAmount = 0;
            if (cla.Particular == "Other") {
                if ($scope.txt_approxTotAmount > 0) {
                    angular.forEach($scope.Tour_Details, function (dd) {
                        if (dd.chk_amt == true && dd.totaL_amt > 0 && dd.Particular != "Other") {
                            txt_approxTotAmount = (Number(txt_approxTotAmount) + Number(dd.totaL_amt))
                        }
                    })

                    taxpercentage = ((Number(txt_approxTotAmount) / 100) * 5);
                    if (taxpercentage < cla.totaL_amt) {
                        swal("Other Amount Must Be 5% Total Amount  !");
                        cla.totaL_amt = "";
                        cla.enableflag = false;
                    }

                }
                else {
                    cla.enableflag = false;
                    cla.totaL_amt = 0;
                    swal("Total Amount Not  Calculated !");
                }
                $scope.countDetails();
                return;
            }
            //else if (cla.Particular == "Travel") {
            //    $scope.countDetails();
            //    return;
            //}

            else {
                if (cla.TotalAmt < cla.totaL_amt) {
                    swal("Total Amount Not Greater Than Actual Amount !");
                    cla.totaL_amt = "";

                }
            }
            $scope.countDetails();
        }

        $scope.materaldocuupload = [{ id: 'materal' }];
        // $('#tadadocument').modal('hide');
        $scope.DcGeneratrate = function (user) {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.obj.MI_Id = "";
                $scope.INVACO_CheckoutDate = new Date(user.IER_Plan_StartDate);

            });
            $('#tadadct').modal('show');


        }
        $scope.ApprovalRemarks = function (item) {
            $scope.allowense = [];
            $scope.txt_approxTotAmountNew = "";
            $scope.RepeatDetails = [];
            $scope.RepeatDetailsNew = [];
            $scope.Tour_Details = [];
            $scope.IER_IDNew = "";
            $scope.orderFlag = false;
            $scope.Datacategorynew = [];
            $scope.IER_IDNew = item.IER_ID;
            $scope.plandate = new Date(item.IER_Plan_StartDate);
            $scope.planenddate = new Date(item.IER_Plan_EndDate);
            var data = {
                "IER_ID": item.IER_ID
            };
            apiService.create("TADA_Advance_Tour/getView", data).then(function (promise) {
                if (promise.editdataOthelist != null && promise.editdataOthelist.length > 0) {
                    angular.forEach(promise.editdataOthelist, function (dd) {
                        $scope.RepeatDetailsNew.push({
                            IER_ID: dd.IER_ID,
                            IVRMMS_Id: dd.IVRMMS_Id,
                            IVRMMS_Name: dd.IVRMMS_Name,
                            IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                            IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                            IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                            IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                            IRPD_Order: dd.IRPD_Order
                        })

                    })
                    $scope.maxstartdateNew = new Date(promise.editdataOthelist[0].IER_Plan_StartDate);
                    $scope.maxEndDateNew = new Date(promise.editdataOthelist[0].IER_Plan_EndDate);
                    $scope.Datacategorynew = promise.getPlanerdata;
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
                    if (promise.vtadaaA_Id > 0) {
                        $scope.VTADAAA_Id = promise.vtadaaA_Id;
                    }
                    else {
                        $scope.VTADAAA_Id = 0;
                    }
                    if (promise.tour_Details != null && promise.tour_Details.length > 0) {
                        $scope.allowense = promise.tour_Details;
                        var Totalamount = 0;
                        if ($scope.allowense != null && $scope.allowense.length > 0) {
                            angular.forEach($scope.allowense, function (dd) {
                                Totalamount = (Number(Totalamount) + Number(dd.TotalAmt))

                            })
                        }
                        $scope.txt_approxTotAmountNew = Totalamount;
                    }
                    $('#leadtada').modal('show');

                }
                else {
                    swal("Leads Are  Not  Create !");
                }

            });
        }
        //$scope.editapply = function (VTADAAA_Id) {
        //    $scope.allowense = [];
        //    var data = {
        //        "VTADAAA_Id": VTADAAA_Id
        //    };
        //    apiService.create("TADA_Advance_Apply/edit", data).then(function (promise) {
        //        if (promise.allowense != null && promise.allowense.length > 0) {
        //            $scope.allowense = promise.allowense;
        //        }
        //    });
        //};

        //DcGeneratrate
        $scope.OnItemchange = function (materal) {
            angular.forEach($scope.get_items, function (itm) {
                // if (materal.INVMI_Id.INVMI_Id == itm.INVMI_Id) {
                if (materal.INVMI_Id.INVSTO_Id == itm.INVSTO_Id) {
                    materal.INVSTO_AvaiableStock = "";
                    materal.INVMI_Id_new = "";
                    materal.INVMS_StoreName = "";
                    materal.INVMS_StoreName = itm.INVMS_StoreName;
                    materal.INVSTO_PurchaseDate = new Date(itm.INVSTO_PurchaseDate);
                    materal.INVSTO_SalesRate = itm.INVSTO_SalesRate;
                    materal.INVSTO_AvaiableStock = itm.INVSTO_AvaiableStock;
                    materal.INVMI_Id_new = itm.INVMI_Id;
                    materal.INVMST_Id = itm.INVMST_Id;
                    materal.INVACO_CheckoutDate = new Date($scope.INVACO_CheckoutDate);
                    return;
                }

            });

        }
        $scope.addgrnrows = function (materal) {
            if (materal.INVACO_CheckOutQty > 0 && materal.INVMI_Id_new > 0) {
                var totlqty = 0;
                var availablestok = 0;
                totlqty = parseInt(materal.INVACO_CheckOutQty);
                availablestok = parseInt(materal.INVSTO_AvaiableStock);
                if (totlqty > availablestok) {
                    swal("Please Check Available Stock...!!");
                    // materal.INVACO_CheckOutQty = "";
                    return;
                }
                var newItemNo = $scope.materaldocuupload.length + 1;
                if (newItemNo <= 100) {
                    $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
                }
            }
            else {
                swal("Please Select  Qunatity / Item  !")
            }

        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {               //data
            }
        };
        var minutes;
        var meridiem;
        var hours
        $scope.checkOut = function () {
            //to get time in 12 hr format added by adarsh
            var currentDate = new Date();
            hours = currentDate.getHours();
            minutes = currentDate.getMinutes();
            meridiem = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12 || 12;
            var cuurtimee = hours + ':' + minutes + ' ' + meridiem;
            $scope.obj.INVACO_Time = cuurtimee;

            $scope.INVACO_Tolocation = document.getElementById("INVACO_Tolocation").innerText
            //time function ends------------------//
            $scope.submitted2 = true;
            if ($scope.myForm3.$valid) {
                var data = {};
                $scope.employeename_two = "";
                for (var i = 0; i < $scope.get_employee.length; i++) {
                    if ($scope.get_employee[i].hrmE_Id == $scope.hrmE_Id) {
                        $scope.employeename_two = $scope.get_employee[i].employeename;
                    }
                }
                $scope.itemarray = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (materal) {
                        if (materal.INVMI_Id_new > 0 && materal.INVACO_CheckOutQty > 0) {
                            $scope.itemarray.push({
                                INVMI_Id: materal.INVMI_Id_new,
                                INVACO_CheckoutDate: new Date(materal.INVACO_CheckoutDate).toDateString(),
                                INVACO_CheckOutQty: materal.INVACO_CheckOutQty,
                                INVSTO_SalesRate: materal.INVSTO_SalesRate,
                                INVMST_Id: materal.INVMST_Id,
                                INVACO_ItemRemarks: materal.INVACO_ItemRemarks,
                                INVACO_QuantityRemark: materal.INVACO_QuantityRemark,

                            })
                        }

                    });

                    if ($scope.itemarray != null && $scope.itemarray.length > 0) {

                        data = {

                            "INVACO_ReceivedBy": $scope.employeename_two,
                            "HRME_Id": $scope.hrmE_Id,
                            "itemarray": $scope.itemarray,
                            "MI_Id": $scope.obj.MI_Id,
                            "INVACO_MODE": $scope.obj.INVACO_MODE,
                            "INVACO_ISSUED": $scope.obj.INVACO_ISSUED,
                            "INVACO_Tolocation": $scope.INVACO_Tolocation,
                            "INVACO_CheckOutRemarks": $scope.obj.INVACO_CheckOutRemarks,
                            "INVACO_Vehicalno": $scope.obj.INVACO_Vehicalno,
                            "INVACO_Time": $scope.obj.INVACO_Time,
                            "INVACO_No": $scope.obj.INVACO_No,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                            "returnval": $scope.obj.returnval
                        };
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("CheckOutAssets/GenarateDc", data).then(function (promise) {
                            if (promise.message == "save") {
                                swal("Record Save SuceesFully !");
                            }
                            else if (promise.message == "Notsave") {
                                swal("Record Not Saved !");
                            }
                            else if (promise.message == "admin") {
                                swal("Please Contact Administrtor  !");
                            }
                            $('#tadadct').modal('hide');

                        });
                    }
                    else {
                        swal("Items  Not Found ")
                        return;
                    }

                }
                else {
                    swal("Items  Not Found ")
                    return;
                }

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.onstorechange = function () {
            $scope.transnumbconfigurationsettingsassign = {};
            $scope.get_items = [];
            $scope.get_employee = [];
            $scope.materaldocuupload = [];
            $scope.get_checkoutdclist = [];
            $scope.instlist = [];
            $scope.tempdc = [];
            $scope.materaldocuupload = [{ id: 'materal' }];
            var data = {
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("INV_T_Sales/GetDcItems", data).then(function (promise) {
                if (promise.get_item != null && promise.get_item.length > 0) {
                    $scope.get_items = promise.get_item;
                    $scope.get_employee = promise.get_employee;
                    // $scope.get_checkoutdclist = promise.get_checkoutdclist;
                }
                if (promise.get_checkoutdclist != null && promise.get_checkoutdclist.length > 0) {
                    angular.forEach(promise.get_checkoutdclist, function (materal) {
                        if (materal.hrmE_Id == $scope.hrmE_Id) {
                            $scope.get_checkoutdclist.push(materal);
                        }

                    });
                    //$scope.get_checkoutdclist = promise.get_checkoutdclist;
                    $scope.instlist = promise.get_mi_list;

                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({
                                INVACO_No: ddd.invacO_No,
                                invacO_Tolocation: ddd.invacO_Tolocation,
                                invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                INVACO_MODE: ddd.invacO_MODE,
                                INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                INVACO_Time: ddd.invacO_Time,
                                INVACO_ISSUED: ddd.invacO_ISSUED,
                                invmlO_Status: ddd.invmlO_Status
                            });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVACO_No == ddd.invacO_No) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.tempdc.push({
                                    INVACO_No: ddd.invacO_No,
                                    invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                    invacO_Tolocation: ddd.invacO_Tolocation,
                                    invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                    INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                    INVACO_MODE: ddd.invacO_MODE,
                                    INVACO_ISSUED: ddd.invacO_ISSUED,
                                    INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                    INVACO_Time: ddd.invacO_Time,
                                    invmlO_Status: ddd.invmlO_Status
                                });
                            }
                        }
                    })

                    $scope.mainarray = [];
                    angular.forEach($scope.tempdc, function (ss) {
                        var listdat = [];
                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                            if (ss.INVACO_No == ff.invacO_No) {
                                listdat.push(ff);
                            }

                        })
                        ss.LIST = listdat;
                    })


                }
                if (promise.get_editmember != null && promise.get_editmember.length > 0) {
                    $scope.transnumbconfigurationsettingsassign = promise.get_editmember[0];
                }

            });
        };
        $scope.countDetails = function () {
            //Tour_Details
            var Totalamount = 0;
            if ($scope.Tour_Details != null && $scope.Tour_Details.length > 0) {
                angular.forEach($scope.Tour_Details, function (dd) {
                    if (dd.chk_amt == true && dd.totaL_amt > 0 && dd.enableflag == true) {
                        Totalamount = (Number(Totalamount) + Number(dd.totaL_amt))

                    }
                })
            }
            $scope.txt_approxTotAmount = Totalamount;

        }

        $scope.SaveTada = function () {
            if ($scope.myForm2.$valid) {

                var totalamount = $scope.txt_approxTotAmount;
                $scope.clientnames_rem = [];
                if ($scope.clientnames_rem.length > 0) {
                    angular.forEach($scope.clientnames_rem, function (itmx) {
                        if (true) {
                            $scope.client_names.push({
                                ISMMCLT_Id: itmx.Client_Id,
                                VTADDAMCF_Remarks: itmx.VTADDAMCF_Remarks
                            });
                        }
                    });
                }

                $scope.selecteddetails = [];
                if ($scope.Tour_Details != null && $scope.Tour_Details.length > 0) {
                    angular.forEach($scope.Tour_Details, function (dd) {
                        if (dd.chk_amt == true) {
                            $scope.selecteddetails.push({
                                Type: dd.Particular,
                                Amount: dd.totaL_amt,
                                Remarks: dd.other_Remarks,
                                VTADAAAD_TotalSlots: dd.Slots,
                                VTADAAAD_Slots: dd.Slots,
                            });
                        }
                    });
                }
                else {
                    swal("Not selected Tour Details");
                    return;
                }
                //$scope.selecteddetails
                if ($scope.selecteddetails != null && $scope.selecteddetails.length > 0) {


                }
                else {
                    swal("At least selected One TADA Head");
                    return;
                }
                $scope.savestateamount = [];
                $scope.CategoryPara = [];
                if ($scope.CityDistance.length > 0) {
                    angular.forEach($scope.CityDistance, function (Distance) {
                        if (Distance.VTADAMC_Amount > 0) {
                            $scope.savestateamount.push({
                                VTADAAAC_FromState: Distance.Fromstate,
                                VTADAAAC_ToState: Distance.tostate,
                                VTADAMT_Id: Distance.vtadamT_Id,
                                VTADAMD_Id: Distance.VTADAMD_Id,
                                VTADAAAC_Travel: Distance.vtadamT_Id,
                                VTADAAAC_Amount: Distance.VTADAMC_Amount,
                                IVRMMCT_Id: Distance.IVRMMCT_Id
                            });
                        }
                    });
                }

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



                var VTADAAA_Id = 0;
                if ($scope.VTADAAA_Id > 0) {
                    VTADAAA_Id = $scope.VTADAAA_Id;
                }
                var msg = document.getElementById("emailTada").innerHTML;
                var data = {
                    "VTADAAA_ClientId": 0,
                    "VTADAAA_TotalAppliedAmount": totalamount,
                    "VTADAAA_ToAddress": $scope.txt_Address,
                    "AllowenceArray": $scope.selecteddetails,
                    multiple_client: $scope.client_names,
                    "IER_ID": $scope.IER_IDNew,
                    "VTADAAA_Id": VTADAAA_Id,
                    "message": msg,
                    VMSTADAAmountDTO: $scope.savestateamount,
                    "filelist": $scope.CategoryPara
                };
                apiService.create("TADA_Advance_Tour/SaveTada", data).then(function (promise) {
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
                        $scope.onpageload();
                        $('#tadadocument').modal('hide');
                    }
                    if (promise.returnvalue === false) {
                        if (promise.returnval === "") {
                            swal("please contact Administrator !");
                        }
                    }
                });
            }
        }
        //getView
        $scope.getView = function (item) {
            $scope.RepeatDetails = [];
            $scope.Tour_Details = [];
            $scope.IER_IDNew = "";
            $scope.orderFlag = false;
            $scope.Datacategorynew = [];
            $scope.IER_IDNew = item.IER_ID;
            $scope.plandate = new Date(item.IER_Plan_StartDate);
            $scope.planenddate = new Date(item.IER_Plan_EndDate);
            var data = {
                "IER_ID": item.IER_ID

            };
            apiService.create("TADA_Advance_Tour/getView", data).then(function (promise) {
                //editdataOthelist
                if (promise.editdataOthelist != null && promise.editdataOthelist.length > 0) {
                    //$scope.RepeatDetails = promise.editdataOthelist;
                    angular.forEach(promise.editdataOthelist, function (dd) {
                        $scope.RepeatDetails.push({
                            IER_ID: dd.IER_ID,
                            IVRMMS_Id: dd.IVRMMS_Id,
                            IVRMMS_Name: dd.IVRMMS_Name,
                            IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                            IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                            IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                            IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                            IRPD_Order: dd.IRPD_Order
                        })

                    })

                    //$scope.maxEndDateNew = new Date(promise.editdataOthelist[0].IER_Plan_EndDate);
                    //$scope.plMaxdate.setDate($scope.plMaxdate.getDate());
                    //maxstartdateNew  //maxEndDateNew
                    $scope.maxstartdateNew = new Date(promise.editdataOthelist[0].IER_Plan_StartDate);
                    $scope.maxEndDateNew = new Date(promise.editdataOthelist[0].IER_Plan_EndDate);

                    //  $scope.maxEndDateNew.setDate($scope.plMaxdate.getDate());

                    $scope.Datacategorynew = promise.getPlanerdata;
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
                    if (promise.vtadaaA_Id > 0) {
                        $scope.VTADAAA_Id = promise.vtadaaA_Id;
                    }
                    else {
                        $scope.VTADAAA_Id = 0;
                    }
                    //VTADAAA_Id
                    $('#tadadocument').modal('show');

                }
                else {
                    swal("Leads Are  Not  Create !");
                }

            });


        }

        $scope.sortKey = 'ismS_OrderFlag';
        $scope.sortReverse = false;
        $scope.sortableOptions = {

            stop: function (e, ui) {
                for (var index in $scope.RepeatDetails) {
                    $scope.RepeatDetails[index].ismS_OrderFlag = Number(index) + 1;
                    $scope.RepeatDetails[index].IRPD_Order = Number(index) + 1;
                }
                $scope.orderFlag = false;
            }
            //


        };
        $scope.orderFlag = false;
        $scope.sortableNumber = function () {
            var count = 0;
            $scope.orderFlag = true;
            angular.forEach($scope.RepeatDetails, function (dd) {
                dd.IRPD_Order = "";
                count = count + 1;
                dd.IRPD_Order = count;
            })

        }
        $scope.plandate = "";
        $scope.planenddate = "";
        //
        $scope.LeadIntimate = false;
        $scope.TourPlanDates = function () {
            $scope.plandate = new Date($scope.startdate);
            $scope.planenddate = new Date($scope.enddate);

            var data = {
                "PlanStartDate": new Date($scope.startdate),
                "PlanEndDate": new Date($scope.enddate),

            };
            apiService.create("TADA_Advance_Tour/TourPlanDates", data).then(function (promise) {
                if (promise.duplicate == "1") {
                    swal("Already Applied Tour  Plan These Between  Dates !");
                    $scope.enddate = "";
                    $scope.startdate = "";
                }
                //if ($scope.get_Report != null && $scope.get_Report.length > 0 && $scope.IER_ID == 0) {
                //    angular.forEach($scope.get_Report, function (dd) {
                //        if (dd.Counts == 0) {
                //            swal("Already Applied Tour  Plan Still  u have Not Approved   Old Tour    !");
                //            $scope.enddate = "";
                //            $scope.startdate = "";
                //            return;
                //        }
                //    });
                //}


            });
        }
        $scope.LeadClick = function () {
            $scope.submitted = true;
            $scope.get_employee = [];
            if ($scope.myForm.$valid) {
                $('#leadIntimate').modal('show');
                var data = {
                    "MI_Id": $scope.MI_Id
                };
                apiService.create("INV_T_Sales/GetDcItems", data).then(function (promise) {
                    if (promise.get_employee != null && promise.get_employee.length > 0) {

                        $scope.get_employee = promise.get_employee;
                        // $scope.get_checkoutdclist = promise.get_checkoutdclist;
                    }

                });
            }
            else {
                $scope.LeadIntimate = false;
            }

        }
        $scope.addgrnfile = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 50) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.materaldocuupload = [];
        //$scope.materaldocuupload = [{ id: 'materal' }];

        //$scope.materaldocuupload.push({
        //    ccmail: "kanavisanjeev@gmail.com"
        //})


        $scope.removegrnfiles = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);
        };
        //LeadIntimateApproved
        $scope.LeadIntimateApproved = function () {
            var msg = document.getElementById("leademail").innerHTML;

            var data = {
                "PlanStartDate": new Date($scope.startdate).toDateString(),
                "PlanEndDate": new Date($scope.enddate).toDateString(),

                "PlanName": $scope.PlanName,
                "TourRemark": $scope.TourRemark,
                "returnval": msg
                // "IERC_ID": $scope.IERC_ID

            };

            apiService.create("TADA_Advance_Tour/saveallowence", data).then(function (promise) {
                if (promise.message == "save") {
                    swal("Records Save Sucessfully !");
                }
                else if (promise.message == "notsave") {
                    swal("Records Not Saved  !");
                }
                else if (promise.message == "admin") {
                    swal("Please Contact Administrator !");
                }
                else {
                    swal("Records Not Saved  !");
                }
                $('#leadIntimate').modal('hide');
                $scope.onpageload();
            });

        }
        $scope.CityDistance = [{ itrS_Id1: 'trans12' }];
        $scope.addrowsclient = function () {
            if ($scope.CityDistance.length > 1) {
                for (var x = 0; x === $scope.CityDistance.length; x++) {
                    var id1 = $scope.CityDistance[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.CityDistance.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };
        $scope.remove = function (index, data) {
            var newItemNo = $scope.CityDistance.length - 1;
            $scope.CityDistance.splice(index, 1);
            $scope.travleDetailsCalculation();

        };

        $scope.changeTravelDetails = function (Distance) {
            Distance.Travel_Details = []; Distance.VTADAMD_Id = []; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            if (Distance.vtadamT_Id > 0) {
                var data = {
                    "VTADAMT_Id": Distance.vtadamT_Id
                };
                apiService.create("TADA_Advance_Tour/TravelDetails", data).then(function (promise) {
                    if (promise.travel_Details != null && promise.travel_Details.length > 0) {
                        Distance.Travel_Details = promise.travel_Details;
                    }
                    $scope.travleDetailsCalculation();
                });
            }

        };
        $scope.Tostates = function (Distance, index) {
            Distance.Tostate = []; Distance.Travel_Details = []; Distance.CityList = [];
            Distance.tostate = ""; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = ""; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            Distance.IVRMMCT_Id = "";
            if (Distance.Fromstate > 0) {
                angular.forEach($scope.Fromstate, function (ob) {
                    Distance.Tostate.push(ob);
                });
            }

            $scope.travleDetailsCalculation();

        }
        $scope.onchangecitylist = function (Distance) {
            Distance.Travel_Details = []; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = ""; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            $scope.travleDetailsCalculation();
        }
        $scope.OnloadTostate = function (Distance) {
            Distance.Travel_Details = []; Distance.CityList = []; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = "";
            Distance.VTADAMC_AmountNew = ""; Distance.VTADAMC_Amount = ""; Distance.IVRMMCT_Id = "";
            $scope.VMSTADAAmountDTO = [];
            if ($scope.CityDistance != null && $scope.CityDistance.length > 0) {
                angular.forEach($scope.CityDistance, function (obc) {
                    if (obc.IVRMMCT_Id > 0) {
                        $scope.VMSTADAAmountDTO.push({
                            IVRMMCT_Id: obc.IVRMMCT_Id
                        })
                    }
                });
            }
            if (Distance.tostate > 0) {
                var data = {
                    "Fromstate": Distance.Fromstate,
                    "Tostate": Distance.tostate,
                    "IER_ID": $scope.IER_IDNew,
                    "VMSTADAAmountDTO": $scope.VMSTADAAmountDTO
                };
                apiService.create("TADA_Advance_Tour/GetCity", data).then(function (promise) {
                    if (promise.travel_data != null && promise.travel_data.length > 0) {
                        Distance.CityList = promise.travel_data;
                    }
                    else {
                        swal("Amount Travel not mapped");
                    }
                });
            }

            $scope.travleDetailsCalculation();
        }

        $scope.Getamount = function (Distance) {
            var VRMMDT_Id = 0;
            Distance.VTADAMC_AmountNew = ""; Distance.VTADAMC_Amount = "";
            angular.forEach(Distance.CityList, function (ob) {
                if (ob.IVRMMCT_Id == Distance.IVRMMCT_Id) {
                    VRMMDT_Id = ob.VRMMDT_Id;
                }
            });
            var data = {
                "Fromstate": Distance.Fromstate,
                "Tostate": Distance.tostate,
                "VTADAMT_Id": Distance.vtadamT_Id,
                "VTADAMD_Id": Distance.VTADAMD_Id,
                "VRMMDT_Id": VRMMDT_Id,
            };
            apiService.create("TADA_Advance_Tour/Getamount", data).then(function (promise) {
                if (promise.travelAmount != null && promise.travelAmount.length > 0) {
                    Distance.VTADAMC_AmountNew = promise.travelAmount[0].VTADAMC_Amount;
                    Distance.VTADAMC_Amount = promise.travelAmount[0].VTADAMC_Amount;
                }
                else {
                    swal("Amount Travel not mapped");

                }
                $scope.travleDetailsCalculation();
            });
        };
        $scope.travelAmountDetails = function (Distance) {
            if (Distance.VTADAMC_Amount > 0) {
                if (Distance.VTADAMC_AmountNew < Distance.VTADAMC_Amount) {
                    swal("Total Amount Not Greater Than Travel Amount !");
                    Distance.VTADAMC_Amount = "";

                }
            }
            $scope.travleDetailsCalculation();
        }
        $scope.travleDetailsCalculation = function () {
            var VTADAMC_Amount = 0;
            angular.forEach($scope.CityDistance, function (ob) {
                if (ob.VTADAMC_Amount > 0) {
                    VTADAMC_Amount = (Number(VTADAMC_Amount) + (Number(ob.VTADAMC_Amount)));
                }
            });
            if ($scope.Tour_Details != null && $scope.Tour_Details.length > 0) {
                angular.forEach($scope.Tour_Details, function (ob) {
                    if (ob.Particular == "Travel") {
                        ob.chk_amt = false;
                        ob.totaL_amt = "";
                        ob.TotalAmt = (Number(VTADAMC_Amount));
                        $scope.countDetails();
                        return;
                    }
                });
            }

        };
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd123 = function (input, document) {
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

    }
})();

