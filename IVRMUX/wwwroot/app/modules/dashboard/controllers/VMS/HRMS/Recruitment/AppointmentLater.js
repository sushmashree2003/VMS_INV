(function () {
    'use strict';
    angular
        .module('app')
        .controller('appointmentController', appointmentController);

    appointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function appointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        //$scope.Candidate.hrmC_Id = "";
        //$scope.submitted = true;
        //$scope.submitted2 = true;
        //$scope.submitted3 = true;
        $scope.currentmi_id = 0;
        $scope.salary = true;
        $scope.document = true;
        $scope.hrcA_Id = 0;

        $scope.camelcase = function (str) {
            if (str != undefined && str != null) {
                var array1 = str.split(' ');
                var newarray1 = [];

                for (var x = 0; x < array1.length; x++) {
                    newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
                }
                $scope.stuDobwords = newarray1;
                str = newarray1.join(' ');
            }
            return str;
        };


        //$scope.showModal = function () {
        //    $('#Assetsmap').modal('show');
        //    $scope.loaddata();  

        //};

        //$scope.closeModal = function () {
        //    $('#Assetsmap').modal('hide');
        //};

        $scope.closeModal = function () {
            //$('#Assetsmap').modal('hide');  
            $state.reload();
        };



        //for popup Appintmnets 
        $scope.showModal = function () {
            $('#Assetsmap').modal('show');  // Show the modal

            // Call the API after the modal is shown
            var pageid = 2;
            apiService.getURI("INV_LocationDashboard/getalldetails", pageid).then(function (promise) {

                $scope.floordetails = promise.floordetails;
                if ($scope.floordetailtemp != null && $scope.floordetailtemp.length > 0) {
                    angular.forEach($scope.floordetailtemp, function (flr) {
                        if ($scope.floordetails.length === 0) {
                            $scope.floordetails.push(flr);
                        }
                        else if ($scope.floordetails.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.floordetails, function (rom) {
                                if (rom.INVMLO_Id == flr.INVMLO_Id && rom.Room_Name == flr.Room_Name) {
                                    intcount += 1;
                                }
                            });
                            if (intcount == 0) {
                                $scope.floordetails.push(flr);
                            }
                        }
                    })
                }
                //$scope.employeedetails1 = [];
                //$scope.employeelist = [];
                //if (promise.employeedetails != null && promise.employeedetails.length > 0) {
                //    angular.forEach(promise.employeedetails, function (rom) {
                //        if (rom.HRME_ActiveFlag == true) {
                //            $scope.employeelist.push(rom);
                //        }
                //        else {
                //            $scope.employeedetails1.push(rom);

                //        }
                //    }); 
                //}
                $scope.checkoutemployeelist2 = [];
                $scope.checkoutemployeelist1 = [];
                if (promise.checkoutemployeelist != null && promise.checkoutemployeelist.length > 0) {
                    angular.forEach(promise.checkoutemployeelist, function (rom) {
                        if (rom.hrmE_ActiveFlag == true) {
                            $scope.checkoutemployeelist2.push(rom);
                        }
                        else if (rom.hrmE_ActiveFlag == false) {
                            $scope.checkoutemployeelist1.push(rom);

                        }
                    });
                }
                //$scope.checkoutemployeelist = [];

                //if (promise.checkoutemployeelist != null && promise.checkoutemployeelist.length > 0) {
                //    $scope.checkoutemployeelist = promise.checkoutemployeelist.filter(function (rom) {
                //        return rom.hrmE_ActiveFlag === true;
                //    });
                //}




                //$scope.employeelist = promise.employeedetails;
                //$scope.checkoutemployeelist = promise.checkoutemployeelist;
                $scope.locationdetails = promise.locationdetails;
                console.log($scope.employeelist);

                // Process the room details
                $scope.roomdetails = [];
                if ($scope.floordetails != null && $scope.floordetails.length > 0) {
                    angular.forEach($scope.floordetails, function (flr) {
                        if ($scope.roomdetails.length === 0) {
                            $scope.roomdetails.push(flr);
                        }
                        else if ($scope.roomdetails.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.roomdetails, function (rom) {
                                if (rom.Floor_Name == flr.Floor_Name) {
                                    intcount += 1;
                                }
                            });
                            if (intcount == 0) {
                                $scope.roomdetails.push(flr);
                            }
                        }
                    })
                    console.log($scope.roomdetails);

                    // Adding employee details
                    if ($scope.employeelist !== null && $scope.employeelist.length > 0) {
                        angular.forEach($scope.floordetails, function (loca) {
                            angular.forEach($scope.employeelist, function (emp) {
                                if (loca.INVMLO_Id == emp.INVMLO_Id && loca.INVMLO_LocationRemarks != "Common" && emp.HRME_EmployeeCode != "5855" && emp.HRME_EmployeeCode != "25") {
                                    loca.HRME_Id = emp.HRME_Id;
                                    loca.EmployeeName = emp.EmployeeName;
                                    loca.HRME_Photo = emp.HRME_Photo;
                                    loca.HRMD_DepartmentName = emp.HRMD_DepartmentName;
                                    loca.HRMDES_DesignationName = emp.HRMDES_DesignationName;
                                    loca.INVMLO_LocationRoomName = emp.INVMLO_LocationRoomName;
                                    loca.INVMLO_Id = emp.INVMLO_Id;
                                    loca.HRME_LeftFlag = emp.HRME_LeftFlag;
                                    loca.HRME_EmployeeCode = emp.HRME_EmployeeCode;
                                }
                            })
                        })
                    }
                }
            });
            console.log($scope.url);
        };

        $scope.itemsPerPage = 10;
        $scope.currentPage = 0;
        $scope.obj.search = "";
        $scope.OnClickemployeedetails = function (item) {
            //$scope.HRME_Id = item.HRME_Id
            //5855
            $scope.employeedetails = [];
            if (item.INVMLO_LocationRemarks != 'CRSD001') {
                var data = {
                    "MI_ID": item.MI_ID,
                    "HRME_Id": item.HRME_Id,
                    "Flag": item.Flag,
                    "User_Id": item.INVMLO_Id
                }
                apiService.create("INV_LocationDashboard/getemployeedetail", data).then(function (promise) {
                    $scope.employeedetails = promise.employeedetails;

                    $scope.employeeid = [];
                    if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                        angular.forEach($scope.employeedetails, function (dev) {
                            dev.search = "";
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            }

                            else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push(dev);
                                }
                            }
                        });

                        console.log($scope.employeeid);
                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.employeedetails, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.RecordDetails = $scope.templist;

                        });
                        //
                        $('#departments').modal('show');
                        $scope.TotalgrossAmountvalue = 0;
                        var total = 0;
                        if ($scope.templist != null) {
                            for (var i = 0; i < $scope.templist.length; i++) {
                                var product = $scope.templist[i];
                                total += product.INVTGRN_PurchaseRate;
                            }
                        }
                        $scope.TotalgrossAmountvalue = Math.round(total);

                    }
                    else {
                        swal("Location Is Empty !");
                    }
                });

            }

        }





























        /////*******************Appointments Area Start ***************************//////

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {
                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterjob !== null && promise.masterjob.length > 0) {
                    $scope.masterjob = promise.masterjob;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }

                $scope.currentmi_id = promise.mI_Id;
                $scope.companylist = promise.companylist;
                $scope.departmenlist = promise.departmenlist;
                $scope.candidatelist = promise.candidatelist;
                $scope.earingdeductionlist = promise.earingdeductionlist;
                $scope.detectionList = promise.detectionList;
                $scope.earningList = promise.earningList;
                $scope.arrearList = promise.arrearList;
                $scope.desgnationlist = promise.desgnationlist;
                $scope.departmentlist = promise.departmentlist;

                angular.forEach($scope.earningList, function (value, key) {
                    $scope.earningList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.earningList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrceD_Amount = '0.00';
                        $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.earningList[key].AmountDis = true;
                        $scope.earningList[key].PercentDis = false;
                    }
                    else {
                        $scope.earningList[key].hrceD_Percentage = '0.00';
                        $scope.earningList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = '0.00';
                        $scope.earningList[key].hrmeD_Percent = '0.00';
                        $scope.earningList[key].hrmeD_Details = '';
                        $scope.earningList[key].AmountDis = false;
                        $scope.earningList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.detectionList, function (value, key) {
                    $scope.detectionList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Amount = '0.00';
                        $scope.detectionList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.detectionList[key].AmountDis = true;
                        $scope.detectionList[key].PercentDis = false;
                    }
                    else {
                        $scope.detectionList[key].hrmeD_Percent = '0.00';
                        $scope.detectionList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Percentage = '0.00';
                        $scope.detectionList[key].hrmeD_Details = '';
                        $scope.detectionList[key].AmountDis = false;
                        $scope.detectionList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.arrearList, function (value, key) {
                    $scope.arrearList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.arrearList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrceD_Amount = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.arrearList[key].AmountDis = true;
                        $scope.arrearList[key].PercentDis = false;
                    }
                    else {
                        $scope.arrearList[key].hrceD_Percentage = '0.00';
                        $scope.arrearList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = '0.00';
                        $scope.arrearList[key].hrmeD_Details = '';
                        $scope.arrearList[key].AmountDis = false;
                        $scope.arrearList[key].PercentDis = true;
                    }
                });

                if (promise.grossList[0] != null && promise.grossList[0] != undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hrceD_ActiveFlag = false;
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                }
                else {
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hrceD_ActiveFlag = false;
                }

                $scope.EarningTotal = $scope.getEarningTotal();
                $scope.DeductionTotal = $scope.getDeductionTotal();
                $scope.ArrearTotal = $scope.getArrearTotal();
                $scope.netSalary = "";
                $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                $scope.pagePrivilege = [];
                if (promise.pagePrivilege !== null && promise.pagePrivilege.length > 0) {
                    $scope.pagePrivilege = promise.pagePrivilege;
                    $scope.IVRMRP_AddFlag = $scope.pagePrivilege[0].IVRMRP_AddFlag;
                    $scope.IVRMRP_UpdateFlag = $scope.pagePrivilege[0].IVRMRP_UpdateFlag;
                    $scope.IVRMRP_DeleteFlag = $scope.pagePrivilege[0].IVRMRP_DeleteFlag;
                    $scope.IVRMRP_ReportFlag = $scope.pagePrivilege[0].IVRMRP_ReportFlag;
                    $scope.IVRMRP_ProcessFlag = $scope.pagePrivilege[0].IVRMRP_ProcessFlag;

                }


            });
        };

        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.submitted2 = false;
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.submitted3 = false;
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        //==============================save data
        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                $scope.mrfCand.hrmqC_RecruitmentStatus = "New";
                var data = $scope.mrfCand;
                apiService.create("Appointment/", data).
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
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            //if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {
                            //    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            //}
                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        //=============================Get Designation List
        $scope.Get_Desgination = function (hrmD_Id) {
            var data = {
                "HRMD_Id": hrmD_Id
            };

            apiService.create("Appointment/Get_Desgination", data).then(function (promise) {
                $scope.desgnationlist = promise.desgnationlist;
            });
        };




        $scope.getEmployeeSalaryDetailsByHead = function (data) {



            swal({
                title: "Are you sure?",
                text: "Click Confirm to Updated The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Confirm",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {



                        $scope.dynamicsalarycalculation(data);

                        swal("Record Updated!");


                    }
                    else {
                        swal("Cancelled !");
                    }
                }
            );



        }






        $scope.dynamicsalarycalculation = function (data) {
            $scope.submittedSalarydetails = true;
            //if ($scope.myForm1.$valid) {
            $scope.netSalary = 0;
            $scope.EarningTotal = 0;
            $scope.DeductionTotal = 0;
            $scope.ArrearTotal = 0;

            if ($scope.Candidate === undefined || $scope.Candidate == null) {
                swal("Select Candidate..!!");
                return;
            }

            data.hrcD_Id = $scope.Candidate.hrcD_Id;
            data.mI_Id = $scope.Candidate.mI_Id;

            apiService.create("Appointment/getEmployeeSalaryDetailsByHead", data).then(function (promise) {

                if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                    var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;
                    var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                        return d.hrmeD_EarnDedFlag === 'Earning';
                    });
                    var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                        return d.hrmeD_EarnDedFlag === 'Deduction';
                    });
                    var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                        return d.hrmeD_EarnDedFlag === 'Arrear';
                    });
                    var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                        return d.hrmeD_EarnDedFlag === 'Gross';
                    });
                    if (GrossDetails != null && GrossDetails.length > 0) {
                        $scope.Salary = GrossDetails[0];
                        $scope.Salary.hrceD_ActiveFlag = GrossDetails[0].hrceD_ActiveFlag;

                    } else {
                        $scope.Salary.hrceD_Percentage = '0.00';
                        $scope.Salary.hrceD_Amount = '0.00';
                        $scope.Salary.hrmeD_AppPercent = '0.00';
                        $scope.Salary.hrmeD_Percent = '0.00';
                        $scope.Salary.hrmeD_Details = '';
                        $scope.Salary.hrmeD_Id = 0;
                        $scope.Salary.hrceD_ActiveFlag = false;
                    }

                    $scope.totalEarning = 0;
                    angular.forEach($scope.earningList, function (value, key) {
                        angular.forEach(EarningDetails, function (value1, key1) {
                            if (value.hrmeD_Id == value1.hrmeD_Id) {
                                $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                $scope.earningList[key].hrmE_Id = value1.hrmE_Id;
                                if (value1.hrceD_ActiveFlag === true) {
                                    $scope.totalEarning = $scope.totalEarning + value1.hrceD_Amount;
                                }

                            }
                        });
                    });

                    $scope.totalDeduction = 0;
                    angular.forEach($scope.detectionList, function (value, key) {
                        angular.forEach(DeductionDetails, function (value1, key1) {
                            if (value.hrmeD_Id == value1.hrmeD_Id) {
                                $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                if (value1.hrceD_ActiveFlag === true) {
                                    $scope.totalDeduction = $scope.totalDeduction + value1.hrceD_Amount;
                                }

                            }
                        });
                    });

                    $scope.totalArrear = 0;
                    angular.forEach($scope.arrearList, function (value, key) {
                        angular.forEach(ArrearDetails, function (value1, key1) {
                            if (value.hrmeD_Id == value1.hrmeD_Id) {
                                $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;
                                if (value1.hrceD_ActiveFlag === true) {
                                    $scope.totalArrear = $scope.totalArrear + value1.hrceD_Amount;
                                }

                            }
                        });
                    });

                    $scope.EarningTotal = $scope.getEarningTotal();
                    $scope.DeductionTotal = $scope.getDeductionTotal();
                    $scope.ArrearTotal = $scope.getArrearTotal();
                    $scope.netSalary = "";
                    $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;
                }
            });
            //}
        };

        //===========================Save Appointment data
        $scope.saveAppointmentdata = function () {
            if ($scope.myForm1.$valid) {
                var selectedEarning = [];
                $scope.buttonenable = true;

                if ($scope.Salary.hrmeD_Id > 0) {
                    selectedEarning.push($scope.Salary);
                }
                else {
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hrceD_ActiveFlag = true;
                }

                if ($scope.earningList !== null && $scope.earningList.length > 0) {
                    angular.forEach($scope.earningList, function (earning) {
                        if (earning.hrceD_ActiveFlag) {
                            earning.hrceD_ActiveFlag = true;
                        } else {
                            earning.hrceD_ActiveFlag = false;
                        }
                        selectedEarning.push(earning);
                    });
                }

                var selectedDetection = [];
                if ($scope.detectionList !== null && $scope.detectionList.length > 0) {
                    angular.forEach($scope.detectionList, function (detection) {
                        if (detection.hrceD_ActiveFlag) {
                            detection.hrceD_ActiveFlag = true;
                        } else {
                            detection.hrceD_ActiveFlag = false;
                        }
                        selectedDetection.push(detection);
                    });
                }

                var selectedArrear = [];
                if ($scope.arrearList !== null && $scope.arrearList.length > 0) {
                    angular.forEach($scope.arrearList, function (arrear) {
                        if (arrear.hrceD_ActiveFlag) {
                            arrear.hrceD_ActiveFlag = true;
                        } else {
                            arrear.hrceD_ActiveFlag = false;
                        }
                        selectedArrear.push(arrear);
                    });
                }

                if (selectedEarning.length === 0 && selectedDetection.length === 0) {
                    swal('Kindly select atleast one record from Earning / Deduction');
                    return;
                }

                $scope.Candidate.hrcD_JoiningDate = new Date($scope.Candidate.hrcD_JoiningDate).toDateString();
                $scope.Candidate.hrcD_InterviewDate = new Date($scope.Candidate.hrcD_InterviewDate).toDateString();

                var data = {
                    Employeedto: $scope.Candidate,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear,
                    "userMessage": "Concert",
                    "HRCD_Id": $scope.Candidate.hrcD_Id

                };

                apiService.create("Appointment/saveAppointmentdata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            if ($scope.currentmi_id == $scope.Candidate.mI_Id) {
                                $scope.salary = false;
                                $scope.document = false;

                                if (promise.hrcD_ExitDate !== null) {
                                    $scope.hrcD_ExitDate = new Date(promise.hrcD_ExitDate);
                                }
                                else {
                                    $scope.hrcD_ExitDate = null;
                                }

                                swal("Record Saved Successfully..");
                            }
                            else { swal("Candidate Moved Successfully..!"); }
                        }
                        else if (promise.retrunMsg === "Update") {
                            $scope.salary = false;
                            $scope.document = false;

                            if (promise.hrcD_ExitDate !== null) {
                                $scope.hrcD_ExitDate = new Date(promise.hrcD_ExitDate);
                            }
                            else {
                                $scope.hrcD_ExitDate = null;
                            }

                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
                $scope.buttonenable = false;
                $scope.salary = true;
                $scope.document = true;
            }
        };

        $scope.savesalarydata = function () {
            if ($scope.myFormSalary.$valid) {
                var selectedEarning = [];
                if ($scope.Salary.hrmeD_Id > 0) {
                    selectedEarning.push($scope.Salary);
                }
                else {
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hrceD_ActiveFlag = true;
                }

                if ($scope.earningList !== null && $scope.earningList.length > 0) {
                    angular.forEach($scope.earningList, function (earning) {
                        if (earning.hrceD_ActiveFlag) {
                            earning.hrceD_ActiveFlag = true;
                        } else {
                            earning.hrceD_ActiveFlag = false;
                        }
                        selectedEarning.push(earning);
                    });
                }

                var selectedDetection = [];
                if ($scope.detectionList !== null && $scope.detectionList.length > 0) {
                    angular.forEach($scope.detectionList, function (detection) {
                        if (detection.hrceD_ActiveFlag) {
                            detection.hrceD_ActiveFlag = true;
                        } else {
                            detection.hrceD_ActiveFlag = false;
                        }
                        selectedDetection.push(detection);
                    });
                }

                var selectedArrear = [];
                if ($scope.arrearList !== null && $scope.arrearList.length > 0) {
                    angular.forEach($scope.arrearList, function (arrear) {
                        if (arrear.hrceD_ActiveFlag) {
                            arrear.hrceD_ActiveFlag = true;
                        } else {
                            arrear.hrceD_ActiveFlag = false;
                        }
                        selectedArrear.push(arrear);
                    });
                }

                if (selectedEarning.length === 0 && selectedDetection.length === 0) {
                    swal('Kindly select atleast one record from Earning / Deduction');
                    return;
                }

                var data = {
                    Employeedto: $scope.Candidate,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear
                };

                apiService.create("Appointment/savesalarydata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            //$scope.salary = false;
                            //$scope.document = false;
                            swal("Record Saved Successfully..");
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.buttonenable = false;
        $scope.loaddata = function (cand) {
            $scope.candidatectc = "";
            var data = {
                "HRCD_Id": cand
            };
            //var data = candidate;

            apiService.create("Appointment/getcandidate", data).then(function (promise) {
                if (promise.retrunMsg != null && promise.retrunMsg == "") {
                    //if (promise.candidateDetails[0].hrcD_FatherName !== null && promise.candidateDetails[0].hrcD_FatherName !== "") {
                    //    $scope.buttonenable = true;
                    //    $scope.salary = false;
                    //    $scope.document = false;
                    //}
                    //else {
                    //    $scope.buttonenable = false;
                    //    $scope.salary = true;
                    //    $scope.document = true;
                    //}
                    $scope.Candidate = promise.candidateDetails[0];
                    console.log($scope.Candidate);

                    $scope.gendername = promise.ivrmmG_GenderName;

                    $scope.sonofdaugheterof = $scope.gendername === "MALE" ? 'S/O' : "D/O"

                    $scope.hrcD_SHGenderName = promise.hrcD_SHGenderName;

                    if (promise.candidateDetails[0].hrcD_InterviewDate !== null) {
                        promise.candidateDetails[0].hrcD_InterviewDate = new Date(promise.candidateDetails[0].hrcD_InterviewDate);
                    }
                    else {
                        promise.candidateDetails[0].hrcD_InterviewDate = null;
                    }

                    if (promise.candidateDetails[0].hrcD_JoiningDate !== null) {
                        promise.candidateDetails[0].hrcD_JoiningDate = new Date(promise.candidateDetails[0].hrcD_JoiningDate);
                    }
                    else {
                        promise.candidateDetails[0].hrcD_JoiningDate = null;
                    }

                    if (promise.hrcD_AppDate !== null) {
                        $scope.hrcD_AppDate = new Date(promise.hrcD_AppDate);
                    }
                    else {
                        $scope.hrcD_AppDate = null;
                    }

                    promise.candidateDetails[0].hrcD_FirstName = $scope.camelcase(promise.candidateDetails[0].hrcD_FirstName);
                    promise.candidateDetails[0].hrcD_MiddleName = $scope.camelcase(promise.candidateDetails[0].hrcD_MiddleName);
                    promise.candidateDetails[0].hrcD_LastName = $scope.camelcase(promise.candidateDetails[0].hrcD_LastName);

                    //promise.candidateDetails[0].hrcD_FatherName = $scope.camelcase(promise.candidateDetails[0].hrcD_FatherName);
                    //promise.candidateDetails[0].hrcD_SHName = $scope.camelcase(promise.candidateDetails[0].hrcD_SHName);

                    if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                        var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;

                        $scope.earningDetails1 = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Earning' && d.hrceD_ActiveFlag === true;
                        });
                        $scope.ductionDetails1 = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Deduction' && d.hrceD_ActiveFlag === true;
                        });



                        var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Earning';
                        });
                        var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Deduction';
                        });
                        var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Arrear';
                        });
                        var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_EarnDedFlag === 'Gross';
                        });

                        if (GrossDetails != null && GrossDetails.length > 0) {
                            $scope.Salary = GrossDetails[0];
                            $scope.Salary.hrceD_ActiveFlag = GrossDetails[0].hrceD_ActiveFlag;
                        }
                        else {
                            $scope.Salary.hrceD_Percentage = '0.00';
                            $scope.Salary.hrceD_Amount = '0.00';
                            $scope.Salary.hrmeD_AppPercent = '0.00';
                            $scope.Salary.hrmeD_Percent = '0.00';
                            $scope.Salary.hrmeD_Details = '';
                            $scope.Salary.hrceD_ActiveFlag = false;
                        }

                        //Earning List
                        $scope.totalEarning = 0;
                        angular.forEach($scope.earningList, function (value, key) {
                            angular.forEach(EarningDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.earningList[key].hrcD_Id = value1.hrcD_Id;
                                    if (value1.hrceD_ActiveFlag === true) {
                                        $scope.totalEarning = parseInt($scope.totalEarning) + parseInt(value1.hrceD_Amount);
                                    }
                                }
                            });
                        });

                        //deductionlist
                        $scope.totalDeduction = 0;
                        angular.forEach($scope.detectionList, function (value, key) {
                            angular.forEach(DeductionDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    //  $scope.detectionList[key].Selected = true;
                                    $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.detectionList[key].hrcD_Id = value1.hrcD_Id;
                                    if (value1.hrceD_ActiveFlag === true) {
                                        $scope.totalDeduction = parseInt($scope.totalDeduction) + parseInt(value1.hrceD_Amount);
                                    }
                                }
                            });
                        });

                        //arrearlist
                        $scope.totalArrear = 0;
                        angular.forEach($scope.arrearList, function (value, key) {
                            angular.forEach(ArrearDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    // $scope.arrearList[key].Selected = true;
                                    $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.arrearList[key].hrcD_Id = value1.hrcD_Id;
                                    if (value1.hrceD_ActiveFlag === true) {
                                        $scope.totalArrear = parseInt($scope.totalArrear) + parseInt(value1.hrceD_Amount);
                                    }
                                }
                            });
                        });

                        $scope.EarningTotal = $scope.getEarningTotal();
                        $scope.DeductionTotal = $scope.getDeductionTotal();
                        $scope.ArrearTotal = $scope.getArrearTotal();
                        $scope.netSalary = "";
                        $scope.netSalary = parseInt($scope.EarningTotal + $scope.ArrearTotal) - parseInt($scope.DeductionTotal);
                    }

                    if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                        $scope.basicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Basic Pay';
                        });

                        $scope.hraamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'HRA';
                        });

                        $scope.conveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Conveyance Allowance';
                        });

                        $scope.medicalamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Medical Allowance';
                        });

                        $scope.mobileamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Mobile Allowance';
                        });

                        $scope.otherallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'other allowance';
                        });

                        $scope.ccaamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'CCA';
                        });

                        $scope.outstationamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'OUTSTATION';
                        });

                        $scope.grossamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Gross';
                        });

                        $scope.pfamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PF';
                        });

                        $scope.esiamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'ESI';
                        });

                        $scope.ptamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PT';
                        });

                        $scope.medicalinsuranceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Medical Insurance';
                        });

                        $scope.foodamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Food';
                        });

                        $scope.esicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'ESI';
                        });

                        if ($scope.esicamount.length > 0) {
                            if ($scope.esicamount[0].hrceD_Amount > 0) {
                                $scope.esicamountscnd = Math.round($scope.basicamount[0].hrceD_Amount * (3.25 / 100));
                            }
                            else {
                                $scope.esicamountscnd = 0;
                            }
                        }
                        else {
                            $scope.esicamountscnd = 0;
                        }

                        $scope.pfonbasicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PF';
                        });

                        if ($scope.pfonbasicamount.length > 0) {
                            if ($scope.pfonbasicamount[0].hrceD_Amount > 0) {
                                $scope.pfonbasicamountcnd = 1950;
                            }
                            else {
                                $scope.pfonbasicamountcnd = 0;
                            }
                        }
                        else {
                            $scope.pfonbasicamountcnd = 0;
                        }

                        $scope.performancebonusamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PB' || d.hrmeD_Name === 'Performance Bouns'
                                ;
                        });

                        $scope.monitoryallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'MB' || d.hrmeD_Name === 'Monitory Bonus';
                        });

                        $scope.localconveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Local Conveyance';
                        });

                        $scope.ctcamount = Math.round($scope.netSalary * 12);

                        if ($scope.totalArrear === 0) {
                            var pb_amount = 0;
                            var mb_amount = 0;
                            if ($scope.performancebonusamount !== null && $scope.performancebonusamount.length > 0) {
                                pb_amount = $scope.performancebonusamount[0].hrceD_Amount;
                            }

                            if ($scope.monitoryallowanceamount !== null && $scope.monitoryallowanceamount.length > 0) {
                                mb_amount = $scope.monitoryallowanceamount[0].hrceD_Amount;
                            }

                            $scope.totalArrear = pb_amount + mb_amount;
                        }
                    }

                    if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                        $scope.masterQualification = promise.masterQualification;
                    }

                    if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                        $scope.masterCaste = promise.masterCaste;
                    }

                    if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                        $scope.mastermaritalstatus = promise.mastermaritalstatus;
                    }

                    $scope.currentdate = new Date();

                    $scope.companydetails = promise.companydetails[0];

                    if (promise.letterdetails.length > 0) {
                        $scope.hrcA_Id = promise.letterdetails[0].hrcA_Id;
                        $scope.documentnameone = promise.letterdetails[0].hrcA_FirstDocName;

                        if (promise.letterdetails[0].hrcA_FirstDocDate !== null) {
                            promise.letterdetails[0].hrcA_FirstDocDate = new Date(promise.letterdetails[0].hrcA_FirstDocDate);
                        }
                        else {
                            promise.letterdetails[0].hrcA_FirstDocDate = null;
                        }

                        $scope.documentnametwo = promise.letterdetails[0].hrcA_SecDocName;

                        if (promise.letterdetails[0].hrcA_SecDocDate !== null) {
                            promise.letterdetails[0].hrcA_SecDocDate = new Date(promise.letterdetails[0].hrcA_SecDocDate);
                        }
                        else {
                            promise.letterdetails[0].hrcA_SecDocDate = null;
                        }
                        $scope.candidatectc = "";
                        $scope.NetAmountInwords = "";
                        $scope.first_doc_date = promise.letterdetails[0].hrcA_FirstDocDate;
                        $scope.second_doc_date = promise.letterdetails[0].hrcA_SecDocDate;
                        $scope.candidatectc = promise.letterdetails[0].hrcA_AnnualCTC;
                        $scope.monthlyctc = promise.letterdetails[0].hrcA_MonthlyCTC;
                        $scope.apprefno = promise.letterdetails[0].hrcA_AppointmentRefNo;
                        $scope.ackrefno = promise.letterdetails[0].hrcA_AcknowledgementRefNo;
                        $scope.NetAmountInwords = $scope.amountinwords($scope.candidatectc) + 'Rupees Only.';
                    }

                    if (promise.candidateDetails[0].mI_Id == 16) { $scope.companyowner = "Mr.Govindaiah M "; $scope.companycode = "VKS"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 17) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "VTS"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 20) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "UNNATHI"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 21) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "SMART"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 22) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "MARGA"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 23) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "PACIFIC"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 24) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "GLOBAL"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    if (promise.candidateDetails[0].mI_Id == 27) { $scope.companyowner = "Mr.Govindaiah M"; $scope.companycode = "AKSHAYA"; $scope.companydesg = "Manager -Accounts and Finance"; }
                    $scope.buttonenable = false;


                }
                else {
                    swal("Record Not Found");
                }
            });
        };

        $scope.loadsalary = function (cand) {
            if (cand != undefined && cand != null) {
                var data = {
                    "HRCD_Id": cand
                };
                apiService.create("Appointment/getcandidate", data).then(function (promise) {
                    if (promise.retrunMsg == "") {
                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;
                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });
                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });
                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });
                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });

                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                $scope.Salary.hrceD_ActiveFlag = GrossDetails[0].hrceD_ActiveFlag;
                            }
                            else {
                                $scope.Salary.hrceD_Percentage = '0.00';
                                $scope.Salary.hrceD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrceD_ActiveFlag = true;
                            }

                            //Earning List
                            $scope.totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {
                                angular.forEach(EarningDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.earningList[key].hrcD_Id = value1.hrcD_Id;
                                        if (value1.hrceD_ActiveFlag === true) {
                                            $scope.totalEarning = parseInt($scope.totalEarning) + parseInt(value1.hrceD_Amount);
                                        }
                                    }
                                });
                            });

                            //deductionlist
                            $scope.totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {
                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.detectionList[key].hrcD_Id = value1.hrcD_Id;
                                        if (value1.hrceD_ActiveFlag === true) {
                                            $scope.totalDeduction = parseInt($scope.totalDeduction) + parseInt(value1.hrceD_Amount);
                                        }
                                    }
                                });
                            });

                            //arrearlist
                            $scope.totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {
                                angular.forEach(ArrearDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.arrearList[key].hrcD_Id = value1.hrcD_Id;
                                        if (value1.hrceD_ActiveFlag) {
                                            $scope.totalArrear = parseInt($scope.totalArrear) + parseInt(value1.hrceD_Amount);
                                        }
                                    }
                                });
                            });

                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();
                            $scope.netSalary = "";
                            $scope.netSalary = parseInt($scope.EarningTotal + $scope.ArrearTotal) - parseInt($scope.DeductionTotal);
                            //$scope.netSalary = parseInt($scope.EarningTotal) - parseInt($scope.DeductionTotal);
                        }

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                            $scope.basicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Basic Pay';
                            });

                            $scope.hraamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'HRA';
                            });

                            $scope.conveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Conveyance  allowance';
                            });

                            $scope.medicalamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MEDICAL ALLOW';
                            });

                            $scope.mobileamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MOB REC';
                            });

                            $scope.otherallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'other allowance';
                            });

                            $scope.ccaamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'CCA';
                            });

                            $scope.outstationamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'OUTSTATION';
                            });

                            $scope.grossamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Gross';
                            });

                            $scope.pfamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PF';
                            });

                            $scope.esiamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'ESI';
                            });

                            $scope.ptamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PT';
                            });

                            $scope.medicalinsuranceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MEDICALINSURANCE';
                            });

                            $scope.foodamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Food';
                            });

                            $scope.esicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'ESI';
                            });

                            if ($scope.esicamount.length > 0) {
                                if ($scope.esicamount[0].hrceD_Amount > 0) {
                                    $scope.esicamountscnd = Math.round($scope.basicamount[0].hrceD_Amount * (3.25 / 100));
                                }
                                else {
                                    $scope.esicamountscnd = 0;
                                }
                            }
                            else {
                                $scope.esicamountscnd = 0;
                            }

                            $scope.pfonbasicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PF';
                            });

                            if ($scope.pfonbasicamount.length > 0) {
                                if ($scope.pfonbasicamount[0].hrceD_Amount > 0) {
                                    $scope.pfonbasicamountcnd = 1950.;
                                }
                                else {
                                    $scope.pfonbasicamountcnd = 0;
                                }
                            }
                            else {
                                $scope.pfonbasicamountcnd = 0;
                            }

                            $scope.performancebonusamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PB' || d.hrmeD_Name === 'Performance Bouns';
                            });

                            $scope.monitoryallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MB' || d.hrmeD_Name === 'Monitory Bonus';
                            });

                            $scope.localconveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'LOCAL CONVEYANCE';
                            });

                            $scope.ctcamount = Math.round($scope.netSalary * 12);
                        }
                    }
                });
            }
        };

        $scope.printInternshipapp1 = function () {
            var innerContents = document.getElementById("internshipappointment").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printToCartapp1 = function () {
            var innerContents = document.getElementById("vapsappointment1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printcheckdeclaration = function () {




            var innerContents = document.getElementById("checkdeclaration").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.printAffudevutexist = function () {
            var innerContents = document.getElementById("affidevit").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printAffudevutnew = function () {
            var innerContents = document.getElementById("affidevitnew").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printToCartapp2 = function () {
            var innerContents = document.getElementById("vapsappointment2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.CancelAppointment = function () {
            $state.reload();
        };

        $scope.printvapssalary = function (vapssalary) {
            var innerContents = document.getElementById("vapssalary").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Cancelsalary = function () {
            $state.reload();
        };

        $scope.printvapmou = function (vapsmou) {
            var innerContents = document.getElementById("vapsmou").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" margin-top: 100px; onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };




        $scope.printvapmouP = function (vapsmouP) {
            var innerContents = document.getElementById("vapsmouP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" margin-top: 100px; onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printvapndaP = function (vapsndaP) {
            var innerContents = document.getElementById("vapsndaP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" margin-top: 100px; onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



        $scope.printacknowledgement = function (vapsacknowledgement) {
            //$scope.first_doc_date = new Date($scope.first_doc_date);
            //$scope.second_doc_date = new Date($scope.second_doc_date);

            var innerContents = document.getElementById("vapsacknowledgement").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printprobationary = function () {
            $('#probationarycertificate1').modal('show');
        };


        $scope.generateprobationarycertificate = function () {
            if ($scope.probationaryForm.$valid) {
                var innerContents = document.getElementById("probationarycertificate").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Kindly Select Required field");
            }
        };


        $scope.Cancelmou = function () {
            $state.reload();
        };

        $scope.printvapnda = function (vapsnda) {
            var innerContents = document.getElementById("vapsnda").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Cancelnda = function () {
            $state.reload();
        };

        $scope.getEarningTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.earningList.length; i++) {
                if ($scope.earningList[i].hrceD_ActiveFlag) {
                    var product = $scope.earningList[i];
                    total += product.hrceD_Amount;
                }
            }
            return total;
        };

        $scope.getDeductionTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.detectionList.length; i++) {
                if ($scope.detectionList[i].hrceD_ActiveFlag) {
                    var product = $scope.detectionList[i];
                    total += product.hrceD_Amount;
                }
            }
            return total;
        };

        $scope.getArrearTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.arrearList.length; i++) {
                if ($scope.arrearList[i].hrceD_ActiveFlag) {
                    var product = $scope.arrearList[i];
                    total += product.hrceD_Amount;
                }
            }
            // total = (total + $scope.pfonbasicamountcnd) + $scope.esicamountscnd;
            return total;
        };

        $scope.printDataappoint = function (Employee) {
            var innerContents = document.getElementById('vapsappointment').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.getcompanydetails = function () {
            var data = {
                "MI_Id": $scope.Candidate.mI_Id
            };

            apiService.create("Appointment/getcompanydetails", data).then(function (promise) {
                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }

                $scope.desgnationlist = promise.desgnationlist;
                $scope.departmentlist = promise.departmentlist;
                $scope.companydetails = promise.companydetails[0];

                $scope.earingdeductionlist = promise.earingdeductionlist;
                $scope.detectionList = promise.detectionList;
                $scope.earningList = promise.earningList;
                $scope.arrearList = promise.arrearList;

                angular.forEach($scope.earningList, function (value, key) {
                    $scope.earningList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.earningList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrceD_Amount = '0.00';
                        $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.earningList[key].AmountDis = true;
                        $scope.earningList[key].PercentDis = false;
                    }
                    else {
                        $scope.earningList[key].hrceD_Percentage = '0.00';
                        $scope.earningList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = '0.00';
                        $scope.earningList[key].hrmeD_Percent = '0.00';
                        $scope.earningList[key].hrmeD_Details = '';
                        $scope.earningList[key].AmountDis = false;
                        $scope.earningList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.detectionList, function (value, key) {
                    $scope.detectionList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Amount = '0.00';
                        $scope.detectionList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.detectionList[key].AmountDis = true;
                        $scope.detectionList[key].PercentDis = false;
                    }
                    else {
                        $scope.detectionList[key].hrmeD_Percent = '0.00';
                        $scope.detectionList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Percentage = '0.00';
                        $scope.detectionList[key].hrmeD_Details = '';
                        $scope.detectionList[key].AmountDis = false;
                        $scope.detectionList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.arrearList, function (value, key) {
                    $scope.arrearList[key].hrceD_ActiveFlag = false;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.arrearList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrceD_Amount = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.arrearList[key].AmountDis = true;
                        $scope.arrearList[key].PercentDis = false;
                    }
                    else {
                        $scope.arrearList[key].hrceD_Percentage = '0.00';
                        $scope.arrearList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = '0.00';
                        $scope.arrearList[key].hrmeD_Details = '';
                        $scope.arrearList[key].AmountDis = false;
                        $scope.arrearList[key].PercentDis = true;
                    }
                });

                if (promise.grossList[0] != null && promise.grossList[0] != undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hrceD_ActiveFlag = false;
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                }
                else {
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hrceD_ActiveFlag = true;
                }

                $scope.EarningTotal = $scope.getEarningTotal();
                $scope.DeductionTotal = $scope.getDeductionTotal();
                $scope.ArrearTotal = $scope.getArrearTotal();
                $scope.netSalary = "";
                $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                $scope.Candidate.hrcD_Department = "";
                $scope.Candidate.hrcD_Designation = "";
                $scope.Candidate.hrmC_Id = "";
                $scope.Candidate.hrcD_MaritalStatus = "";
                $scope.Candidate.hrcD_CasteId = "";
                $scope.Candidate.hrcD_SHGender = "";
                //$scope.submitted = false;
            });
        };

        $scope.saveappointmenttab = function () {
            if ($scope.myForm3.$valid) {
                $scope.buttonenable = true;

                $scope.firstdocdate = new Date($scope.first_doc_date).toDateString();
                $scope.seconddocdate = new Date($scope.second_doc_date).toDateString();

                var data = {
                    Employeedto: $scope.Candidate,
                    "HRCA_Id": $scope.hrcA_Id,
                    "HRCA_FirstDocName": $scope.documentnameone,
                    "HRCA_FirstDocDate": $scope.firstdocdate,
                    "HRCA_SecDocName": $scope.documentnametwo,
                    "HRCA_SecDocDate": $scope.seconddocdate,
                    "HRCA_AnnualCTC": $scope.candidatectc,
                    "HRCA_MonthlyCTC": $scope.monthlyctc,
                    "HRCA_AppointmentRefNo": $scope.apprefno,
                    "HRCA_AcknowledgementRefNo": $scope.ackrefno,
                    "welcomenotice": $scope.welcomenotice
                };

                apiService.create("AddCandidateVMS/saveAppointmenttab", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            $scope.candidatetype = promise.candidatetype;
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            if ($scope.currentmi_id == $scope.Candidate.mI_Id) {
                                $scope.salary = false;
                                $scope.document = false;
                                $scope.candidatetype = promise.candidatetype;
                                swal("Record Saved Successfully..");
                            }
                            else { swal("Candidate Moved Successfully..!"); }
                        }
                        else if (promise.retrunMsg === "Update") {
                            $scope.salary = false;
                            $scope.document = false;
                            $scope.candidatetype = promise.candidatetype;
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
                $scope.buttonenable = false;
                $scope.salary = true;
                $scope.document = true;
            }
        };

        //var th = ['', 'thousand', 'million', 'billion', 'trillion'];
        //var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        //var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        //var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];


        //function toWords(s) {
        //    s = s.toString();
        //    s = s.replace(/[\, ]/g, '');
        //    if (s != parseFloat(s)) return 'not a number';
        //    var x = s.indexOf('.');
        //    if (x == -1) x = s.length;
        //    if (x > 15) return 'too big';
        //    var n = s.split('');
        //    var str = '';
        //    var sk = 0;
        //    for (var i = 0; i < x; i++) {
        //        if ((x - i) % 3 == 2) {
        //            if (n[i] == '1') {
        //                str += tn[Number(n[i + 1])] + ' ';
        //                i++;
        //                sk = 1;
        //            }
        //            else if (n[i] != 0) {
        //                str += tw[n[i] - 2] + ' ';
        //                sk = 1;
        //            }
        //        }
        //        else if (n[i] != 0) {
        //            str += dg[n[i]] + ' ';
        //            if ((x - i) % 3 == 0) str += 'hundred ';
        //            sk = 1;
        //        }


        //        if ((x - i) % 3 == 1) {
        //            if (sk) str += th[(x - i - 1) / 3] + ' ';
        //            sk = 0;
        //        }
        //    }
        //    if (x != s.length) {
        //        var y = s.length;
        //        str += 'point ';
        //        for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
        //    }
        //    return str.replace(/\s+/g, ' ');
        //}





        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++, j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }





        $scope.onDesignationChange = function () {
            $scope.Candidate.hrcD_NatureOfWork = "";
            $scope.Candidate.hrcD_ScopeOfService = "";
            $scope.get_scopenature = []
            if ($scope.Candidate.hrcD_Department != undefined && $scope.Candidate.hrcD_Designation != undefined) {
                var data = {
                    "HRMD_DepartmentName": $scope.Candidate.hrcD_Department,
                    "HRMDES_DesignationName": $scope.Candidate.hrcD_Designation
                };

                apiService.create("Appointment/On_Desgination", data).then(function (promise) {
                    if (promise.get_scopenature != null && promise.get_scopenature.length > 0) {
                        $scope.get_scopenature = promise.get_scopenature;
                        $scope.Candidate.hrcD_NatureOfWork = $scope.get_scopenature[0].HRMNS_NatureWork,
                            $scope.Candidate.hrcD_ScopeOfService = $scope.get_scopenature[0].HRMNS_ScopeService,
                            $scope.HRMNS_ApprovedStatus = $scope.get_scopenature[0].HRMNS_ApprovedStatus
                    }

                });
            }
        };










    }
})();