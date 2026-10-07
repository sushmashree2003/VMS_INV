(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Lead_negotiationController', Sales_Lead_negotiationController);

    Sales_Lead_negotiationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function Sales_Lead_negotiationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {

        $scope.checkbox1 = false;
        $scope.checkbox2 = false;
        $scope.leadnegotiation = [];

        $scope.objs = {
            ISMCLTPRPL_Per: 0 // Initialize as a number
        };

        $scope.LeadModuleData11 = [];
        $scope.LeadModuleData22 = [];
        //$scope.objs = {};
       // $scope.objs.FTI_Id = "";
        //$scope.objs = {
        //    FTI_Id: ""
        //};
        $scope.modulesListNew = [];
        $scope.obj = {};
        $scope.sortReverse = true;
        $scope.editflag = false;
        $scope.updateflag = false;
        $scope.date = new Date();
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.itemsPerPageNew = 10;
        $scope.currentPageNew = 1;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.searchValueDeptd = "";
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addOBrows = function () {
            //$scope.submitted = true;
            //if ($scope.myForm.$valid) {
            if ($scope.transrows.length > 1) {
                for (var i = 0; i === $scope.transrows.length; i++) {
                    var id = $scope.transrows[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
          
        };
        $scope.removeOBrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        $scope.getAllleadtarget = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var id = 2;
            apiService.getURI("Lead_Target_Mapping/OnloadGetDetailsnego", id).
                then(function (promise) {

                    if (promise.leadnegotiation != null && promise.leadnegotiation.length > 0) {
                        $scope.leadnegotiation = promise.leadnegotiation;
                        angular.forEach($scope.leadnegotiation, function (user) {
                            user.transrows = [];
                            user.checkbox1 = true;
                            user.checkbox2 = false;
                        });
                    }
                    $scope.leadnegotiation = promise.leadnegotiation;
                    $scope.leadnegotiation_details = promise.leadnegotiation_details;
                    $scope.yearlist = promise.yearlist;
                    $scope.getinstallment = promise.getinstallment;

                    $scope.modulesListNew = promise.getLeadtarget_Mapping;
                    //

                });
        };

        $scope.updateCheckbox2 = function (user) {
            
            if (user.checkbox1) {
                user.checkbox2 = false;
            }
            else {
                user.checkbox2 = true;
            }
           
        };

        $scope.updateCheckbox1 = function (user) {
            
            if (user.checkbox2) {
                user.checkbox1 = false;
            }
            else {
                user.checkbox1 = true;
            }
        };


        $scope.togchkbxdept = function () {
            $scope.ISMSLE_Id = $scope.leadnegotiation.every(function (itm) {
                return itm.selected;
            });


        };
        // $('#editDemoN').modal('show');
        $scope.addpayment = function (obj) {

            

            if (obj.ISMSLE_Id != "" && obj.ISMSLE_Id != undefined) {

                $scope.transrowsnew = [];
                var id = obj.ISMSLE_Id;
                //$scope.transrowsnew  = $scope.leadnegotiation.find(function (record) {
                //    return record.ISMSLE_Id === id;
                //});
                $scope.transrowsnew = $scope.leadnegotiation
                    .find(function (record) {
                        return record.ISMSLE_Id === id;
                    }).transrows;
             
                if ($scope.transrowsnew != undefined && $scope.transrowsnew != null && $scope.transrowsnew.length > 0) {

                    $scope.transrows = $scope.transrowsnew;
                    angular.forEach($scope.transrows, function (row, index) {

                        if (index < $scope.transrows.length) {
                            //var leadRow = record.tempdata[index];


                            $scope.transrows[index].ISMCLTPRP_Year = row.ISMCLTPRP_Year || "";
                            $scope.transrows[index].FTI_Id = row.FTI_Id || "";
                           // $scope.transrows[index].ISMCLTPRP_PaymentDate = row.ISMCLTPRP_PaymentDate || new Date();
                            $scope.transrows[index].ISMCLTPRP_PaymentDate =
                                row.ISMCLTPRP_PaymentDate ? new Date(row.ISMCLTPRP_PaymentDate) : new Date();


                            $scope.transrows[index].ISMCLTPRPL_Per = row.ISMCLTPRPL_Per || "";
                        }
                    });
                    $('#Payment').modal('show');
                    return;
                    //$scope.obj.LeadId = 0;
                } else {
                    $scope.obj.LeadId = 0;
                    $('#Payment').modal('show');

                    $scope.obj.LeadId = obj.ISMSLE_Id;
                    var data = {
                        "ISMSLE_Id": obj.ISMSLE_Id,
                    }
                    apiService.create("Lead_Target_Mapping/OnChangeLTM_Institution", data).then(function (promise) {
                        if (promise.leadlist != null && promise.leadlist.length > 0) {
                            $scope.transrows = promise.leadlist;
                            angular.forEach($scope.transrows, function (emp) {
                                emp.ISMCLTPRP_PaymentDate = new Date(emp.ISMCLTPRP_PaymentDate);


                            });
                        }

                    });

                   // $scope.obj.LeadId = 0;
                }

                
            }

             


           // //$scope.objs = {
           // //    FTI_Id: ""
           // //};
           // $scope.obj.LeadId = 0; 
           // $('#Payment').modal('show');       

           //// if ($scope.enteredData.length == 0 || $scope.enteredData == null) {
           //     $scope.obj.LeadId = obj.ISMSLE_Id;
           //     var data = {
           //         "ISMSLE_Id": obj.ISMSLE_Id,
           //     }
           //     apiService.create("Lead_Target_Mapping/OnChangeLTM_Institution", data).then(function (promise) {
           //         if (promise.leadlist != null && promise.leadlist.length > 0) {
           //             $scope.transrows = promise.leadlist;
           //             angular.forEach($scope.transrows, function (emp) {
           //                 emp.ISMCLTPRP_PaymentDate = new Date(emp.ISMCLTPRP_PaymentDate);


           //             });
           //         }

           //     });
           //// }
           // //$scope.obj.LeadId = obj.ISMSLE_Id;          
           // //var data = {
           // //    "ISMSLE_Id": obj.ISMSLE_Id,
           // //}
           // //apiService.create("Lead_Target_Mapping/OnChangeLTM_Institution", data).then(function (promise) {
           // //    if (promise.leadlist != null && promise.leadlist.length > 0) {
           // //        $scope.transrows = promise.leadlist;
           // //        angular.forEach($scope.transrows, function (emp) {
           // //            emp.ISMCLTPRP_PaymentDate = new Date(emp.ISMCLTPRP_PaymentDate);
                      

           // //        });
           // //    }
               
           // //});

           




        }
        //AddToPaymentMou
        $scope.submitted1 = false;

        $scope.enteredData = [];
        $scope.AddToPaymentMou = function () {  
            $scope.enteredData = [];
          
          //  $scope.objs.FTI_Id = "";
            if ($scope.dataform1.$valid) {
                $scope.submitted1 = true;
                let duplicateFound = false; // Flag to detect duplicates
                //AddToPayment          
                angular.forEach($scope.leadnegotiation, function (emp) {
                    if ($scope.obj.LeadId == emp.ISMSLE_Id) {
                        emp.transrows = []; 
                        for (var i = 0; i < $scope.transrows.length; i++) {
                            // $scope.getinstallment
                            var FMI_Id = 0;
                            angular.forEach($scope.getinstallment, function (dev) {
                                if ($scope.transrows[i].FTI_Id == dev.ftI_Id) {
                                    FMI_Id = dev.fmI_Id;
                                }
                            });

                            //var isDuplicate = emp.transrows.some(function (existingRow) {
                            //    // Normalize the date to avoid time-related issues
                            //    var existingDate = new Date(existingRow.ISMCLTPRP_PaymentDate).toDateString();
                            //    var newDate = new Date($scope.transrows[i].ISMCLTPRP_PaymentDate).toDateString();

                            //    return (
                            //        existingRow.ISMCLTPRPL_Per == $scope.transrows[i].ISMCLTPRPL_Per &&
                            //        existingRow.FTI_Id == $scope.transrows[i].FTI_Id &&
                            //        existingDate == newDate
                            //    );
                            //});

                            //if (isDuplicate) {
                            //    duplicateFound = true;
                            //    swal("Same Data Can't be Entered.");  // SweetAlert message
                            //    return false;  // Stop the loop and prevent further processing
                            //}


                            emp.transrows.push({
                                FTI_Id: $scope.transrows[i].FTI_Id,
                                ISMCLTPRP_Year: $scope.transrows[i].ISMCLTPRP_Year,
                                ISMCLTPRP_InstallmentAmt: 0,
                                ISMCLTPRP_Year: $scope.transrows[i].ISMCLTPRP_Year,
                                ISMCLTPRP_PaymentDate: new Date($scope.transrows[i].ISMCLTPRP_PaymentDate).toDateString(),
                                ISMCLTPRP_BalanceAmt: 0,
                                ISMCLTPRP_ExcessAmt: 0,
                                ISMCLTPRP_InstallmentName: $scope.transrows[i].ftI_Name,
                                FMI_Id: FMI_Id,
                                ISMCLTPRPL_Per: $scope.transrows[i].ISMCLTPRPL_Per,

                            })
                           

                           
                            $scope.enteredData.push({
                                ISMSLE_Id: emp.ISMSLE_Id, 
                                    FTI_Id: $scope.transrows[i].FTI_Id,
                                    ISMCLTPRP_Year: $scope.transrows[i].ISMCLTPRP_Year,
                                    ISMCLTPRP_InstallmentAmt: 0,  // Set to 0 or provide your own value
                                    ISMCLTPRP_PaymentDate: new Date($scope.transrows[i].ISMCLTPRP_PaymentDate).toDateString(),
                                    ISMCLTPRP_BalanceAmt: 0,  // Set to 0 or provide your own value
                                    ISMCLTPRP_ExcessAmt: 0,  // Set to 0 or provide your own value
                                    ISMCLTPRP_InstallmentName: $scope.transrows[i].ftI_Name,
                                    FMI_Id: FMI_Id,
                                    ISMCLTPRPL_Per: $scope.transrows[i].ISMCLTPRPL_Per
                                });


                         




                        }
                        // emp.transrows.push($scope.transrows);
                      //  $scope.transrows = [];
                    }
                });

                //angular.forEach($scope.leadnegotiation, function (empl) {
                //    empl.tempdataNew = [];
                //    angular.forEach($scope.enteredData, function (skl) {
                       
                //        if (empl.ISMSLE_Id == skl.ISMSLE_Id) {
                            
                //            empl.tempdataNew.push(skl);
                //        }
                //    });

                //});
                angular.forEach($scope.leadnegotiation, function (empl) {

                   

                    // Initialize tempdataNew if it doesn't exist
                    if (!empl.tempdataNew) {
                        empl.tempdataNew = [];
                    }

                    angular.forEach($scope.enteredData, function (skl) {
                        if (empl.ISMSLE_Id == skl.ISMSLE_Id) {

                            empl.tempdataNew.push(skl);
                            
                        }
                    });
                });


                //angular.forEach($scope.leadnegotiation, function (empl) {
                //    empl.tempdata = []; // Clear the tempdata array
                //    if ($scope.obj.LeadId == empl.ISMSLE_Id) {
                //        // Here you push each enteredData into tempdata, not the entire array at once.
                //        empl.tempdata = empl.tempdata.concat($scope.enteredData); // Use concat to merge the data
                //        $scope.enteredData = []; // Reset enteredData after pushing
                //    }
                //});


                console.log("$scope.leadnegotiation", $scope.leadnegotiation);


                $('#Payment').modal('hide');
            }
            else {

                $scope.submitted1 = true;
            }
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
        $scope.savedata = function (temp) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.leadlistarray = [];
                $scope.Project_Payment_Lead = [];
                angular.forEach($scope.leadnegotiation, function (aa) {
                    aa.Project_Payment_Lead = [];
                    if (aa.selected == true) {
                        if (aa.transrows != null && aa.transrows.length > 0) {
                            $scope.leadlistarray.push({
                                ISMSLE_Id: aa.ISMSLE_Id,
                                ISMS_NegotiationFlag: aa.checkbox1,
                                ISMS_MOMFlag: aa.checkbox2,
                                Negotiation_Client: aa.Negotiation_Client,
                                Project_Payment_Lead: aa.transrows,
                                ISMSALENA_Remarks: aa.ISMSLET_ApprovedRemark,
                                ISMSLET_Id: aa.ISMSLET_Id,
                            })
                        }
                        else {
                            swal("Please Add Payment");
                            return;
                            cmd

                        }
                       

                    }
                });
                if ($scope.leadlistarray != null && $scope.leadlistarray.length > 0) {
                    var data = {
                       "leadlistarray": $scope.leadlistarray,
                       // "Project_Payment_Lead": $scope.Project_Payment_Lead                     
                    }
                    apiService.create("Lead_Target_Mapping/savedetails", data).then(function (promise) {
                        if (promise.msg == 'Update') {
                            swal('Record Saved Successfuly');
                           

                        } else {

                           //  swal('Record Save Successfuly');
                            swal('Record Update Failed ');

                        }
                        $state.reload();
                    })
                }
                else {
                    swal("Please Select Atleast One CheckBox/ Add Payment ");
                }
               
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clearid = function () {
            $state.reload();
        }


        $scope.updateNegaotion = function (fll) {
            if (fll.selected12) {
                $scope.fll = fll;
                $('#updateNegaotion').modal('show');
            }
           
        }



        $scope.toggleAll = function (all2) {
            
            var toggleStatus = $scope.obj.all2;
            angular.forEach($scope.leadnegotiation, function (itm) {
             
                itm.selected = toggleStatus;
                
            });
        }

        $scope.optionToggled = function () {
            $scope.obj.all2 = false;
            $scope.checkallm = $scope.leadnegotiation.every(function (itm) {
                return itm.selected;
            });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;

        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;

        };

        //$scope.isOptionsRequired = function () {
        //    if ($scope.optionflag === '') {
        //        return !$scope.getinstallment.some(function (options) {
        //            return options.tgck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};

        $scope.leadmodules = function (fll) {

            $scope.LeadModuleData11 = [];
            $scope.LeadModuleData22 = [];
            $scope.fll = fll;

            var data = {
                "ISMSLE_Id": fll.ISMSLE_Id,
                "ISMSLBOM_Id": fll.ISMSBOM_IdNew     
                //"ISMSLE_Id": 28200,
                //"ISMSLBOM_Id": 53    
            }

            apiService.create("Lead_Target_Mapping/get_quotation", data).then(function (promise) {
                if (promise.leadloaddata1 != null && promise.leadloaddata1.length > 0) {
                    $scope.LeadModuleData11 = promise.leadloaddata1;
                }
                if (promise.leadnegotiation_details != null && promise.leadnegotiation_details.length > 0) {
                    $scope.LeadModuleData22 = promise.leadnegotiation_details;

                }
                $('#leadmodule').modal('show');
            })
            
            
        }
        $scope.all_checkm = function (checkallm) {
            var toggleStatus = checkallm;
            angular.forEach($scope.LeadModuleData22, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

        };

        $scope.togchkbxm = function () {

            $scope.checkallm = $scope.LeadModuleData22.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };

        $scope.SaveModuleData = function (tempNew) {
            
            $scope.bomListdetails = [];

            $scope.LeadModuleListTemp = [];
            angular.forEach($scope.LeadModuleData22, function (itm) {
                if (itm.selected == true) {
                    $scope.bomListdetails.push({ ISMSPS_ID: itm.IVRMM_Id })
                }
            });
            

            if ($scope.bomListdetails != null && $scope.bomListdetails.length > 0) {
                var data = {
                    "ISMSLE_Id": tempNew.ISMSLE_Id,
                    "ISMSLBOM_Id": tempNew.ISMSBOM_IdNew,
                    "bomListdetails": $scope.bomListdetails
                   
                }

                apiService.create("Lead_Target_Mapping/get_quotation", data).then(function (promise) {
                    $scope.LeadModuleData11 = []; $scope.LeadModuleData22 = [];
                    if (promise.leadloaddata1 != null && promise.leadloaddata1.length > 0) {
                        $scope.LeadModuleData11 = promise.leadloaddata1;
                    }
                    if (promise.leadnegotiation_details != null && promise.leadnegotiation_details.length > 0) {
                        $scope.LeadModuleData22 = promise.leadnegotiation_details;

                    }
                    if (promise.msg == "Add") {
                        swal("Modules Added Successfully..");
                    }
                    $('#leadmodule').modal('hide');
                })

            } else {
                swal("Please select Atleast one module..");
            }


           
            

        };

        $scope.isOptionsRequiredNew = function () {
            return !$scope.LeadModuleData22.some(function (options) {
                return options.selected;
            });
        };
        $scope.cancelNew = function () {
            
            angular.forEach($scope.LeadModuleData22, function (itm) {
                itm.selected = false;
            });
        }

    }

})();