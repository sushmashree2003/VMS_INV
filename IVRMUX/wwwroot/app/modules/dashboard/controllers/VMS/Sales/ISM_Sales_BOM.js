
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Sales_BOMController', ISM_Sales_BOMController);
    ISM_Sales_BOMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile', '$filter'];
    function ISM_Sales_BOMController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile, $filter) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
          
        }
        var paginationformasterss;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasterss = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasterss = 0

        }
        $scope.searchvalues = "";
        $scope.returnval = false;
        $scope.loaddata = function () {
            $scope.searchValue = "";
            $scope.serviceListtwo = [];
            $scope.currentPage = 1;
            $scope.currentPages = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPages = paginationformasterss;
            var pageid = 2;
            apiService.getURI("ISM_Sales_BOM/getloaddata", pageid).
                then(function (promise) {
                    
                    $scope.productlist = promise.productlist;
                    $scope.serviceList = promise.serviceList;
                    $scope.boMlist = promise.boMlist;
                    $scope.presentCountgridbomlist = $scope.boMlist.length;

                    angular.forEach(promise.serviceList, function (dev) {
                        if (dev.ismspS_ActiveFlag == true) {
                            $scope.serviceListtwo.push(dev);
                        }
                    });
                    $scope.presentCountgrid = $scope.serviceList.length;

                    if (promise.serviceitemlist.length > 0) {
                        $scope.serviceitemlist = promise.serviceitemlist;
                        $scope.taskid5 = [];
                        angular.forEach($scope.serviceitemlist, function (dev) {
                            if ($scope.taskid5.length === 0) {
                                $scope.taskid5.push({
                                    ISMSPS_ID: dev.ISMSPS_ID,
                                    ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                    ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName

                                });
                            }
                            else if ($scope.taskid5.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.taskid5, function (emp) {
                                    if (emp.ISMSPS_ID === dev.ISMSPS_ID) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.taskid5.push({
                                        ISMSPS_ID: dev.ISMSPS_ID,
                                        ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                    });
                                }
                            }
                        });
                        angular.forEach($scope.taskid5, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.serviceitemlist, function (dd) {
                                if (dd.ISMSPS_ID === ddd.ISMSPS_ID) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.serviceitemlist = $scope.templist;

                        });

                    }
                    else {
                        //  swal('No Data Found!!!');
                        $scope.serviceitemlist = "";
                    }


                });
        };

        //---------------------------------Save--------------------------------------------


        $scope.savedata = function () {
            $scope.submittedSM = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "ISMSPS_ID": $scope.ismspS_ID,
                    "ISMSPS_Service_Name": $scope.ismspS_Service_Name,
                    "ISMSPS_Service_Description": $scope.ismspS_Service_Description,
                    "AbvStrength": $scope.abvStrength,
                }
                apiService.create("ISM_Sales_BOM/savedetails", data).then(function (promise) {
                    if (promise.retrunMsg == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == 'Not Saved') {
                        swal('Record Not Saved');
                    }
                    else if (promise.retrunMsg == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.retrunMsg == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }


                    $scope.submittedUG = false;
                })
            }
            else {
                $scope.submittedUG = true;
            }
        };

        $scope.editt = function (item) {
            $scope.ismspS_Service_Name = item.ismspS_Service_Name;
            $scope.ismspS_Service_Description = item.ismspS_Service_Description;
            $scope.abvStrength = item.abvStrength;
            $scope.ismspS_ID = item.ismspS_ID;
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
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }

        $scope.savedfiles = [];
        $scope.AddFiles = function () {
            $scope.savedfiles = [];
            angular.forEach($scope.materaldocuupload, function (dd) {
                $scope.savedfiles.push({
                    ISMSPSI_ItemsName: dd.itemsName,
                    ISMSPSI_ItemsDescription: dd.description,
                    ISMSPSI_Quantity: dd.quantity,
                    ISMSPSI_Cost: dd.cost,
                    ISMSPSI_AMC_Cost: dd.amccost,
                    ISMSPSI_AMC_Flag: dd.amccostFlag,
                    ISMSPSI_ItemFlag: dd.ISMSPSI_ItemFlag

                })                             
            });
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.ISMSPS_ID = item.ismspS_ID;
            var dystring = "";
            if (item.ismspS_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.ismspS_ActiveFlag == false) {
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
                        apiService.create("ISM_Sales_BOM/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }
        
        $scope.savedataitems = function () {
            $scope.submittedIT = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "ISMSPS_ID": $scope.ismspS_ID,
                    "Iteamsavedfiles": $scope.savedfiles,
                    "returnval": $scope.returnval

                }
                apiService.create("ISM_Sales_BOM/saveiteamsdetails", data).then(function (promise) {
                    if (promise.retrunMsg == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == 'Not Saved') {
                        swal('Record Not Saved');
                    }
                    else if (promise.retrunMsg == 'Duplicate') {
                        swal('Record already exist');
                    }



                    $scope.submittedIT = false;
                })
            }
            else {
                $scope.submittedIT = true;
            }
        };


        $scope.edititem = function (item) {
            $scope.returnval = false;
            $scope.materaldocuupload = [];
            $scope.ismspS_ID = item.ISMSPS_ID;
            $scope.ISMSPS_Service_Name = item.ISMSPS_Service_Name;
            if ($scope.ismspS_ID > 0) {
                $scope.returnval = true;
            }
            angular.forEach(item.serviceitemlist , function (dd) {
                $scope.materaldocuupload.push({
                    itemsName: dd.ISMSPSI_ItemsName,
                    description: dd.ISMSPSI_ItemsDescription,
                    quantity:dd.ISMSPSI_Quantity,
                    cost: dd.ISMSPSI_Cost,
                    amccost: dd.ISMSPSI_AMC_Cost,
                    amccostFlag: dd.ISMSPSI_AMC_Flag,
                    ISMSPSI_ItemFlag: dd.ISMSPSI_ItemFlag
                })
            });

        }
      

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.checked = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.emplist.every(function (options) {
                return options.checked;
            });
        };

        $scope.searchValueE = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.ismspS_Service_Name).indexOf(angular.lowercase($scope.searchValueE)) >= 0;
        };

       
        $scope.savedataBOM = function () {
            $scope.submittedBOM = true;
            if ($scope.myForm3.$valid) {

                $scope.savedfiless = [];
                angular.forEach($scope.serviceListtwo, function (dd) {

                    if (dd.checked == true) {
                        $scope.savedfiless.push({
                            ISMSPS_ID: dd.ismspS_ID,
                            ISMSPS_Service_Name: dd.ismspS_Service_Name,
                        })
                    }
                });
               
                var data = {
                    "ISMSBOM_Id": $scope.ismsboM_Id,
                    "ISMSMPR_Id": $scope.ismsmpR_Id,
                    "serviceListtwo": $scope.savedfiless,
                    "ISMSBOM_BOMName": $scope.ismsbom_bomname,
                    "ISMSBOM_BOMDescription": $scope.ismsboM_BOMDescription

                }
                apiService.create("ISM_Sales_BOM/saveBOMdetails", data).then(function (promise) {
                    if (promise.retrunMsg == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == 'Not Saved') {
                        swal('Record Not Saved');
                    }
                    else if (promise.retrunMsg == 'Duplicate') {
                        swal('Record already exist');
                    }



                    $scope.submittedBOM = false;
                })
            }
            else {
                $scope.submittedBOM = true;
            }
        };

        $scope.viewservice = function (aaaa) {
            var data = {
                "ISMSBOM_Id": aaaa.ismsboM_Id,
            }
            apiService.create("ISM_Sales_BOM/viewservicedetails", data).then(function (promise) {
                //$scope.viewservicedetailslist = promise.viewservicedetailslist;

                if (promise.viewservicedetailslist.length > 0) {
                    $scope.viewservicedetailslist = promise.viewservicedetailslist;
                    $scope.taskid6= [];
                    angular.forEach($scope.viewservicedetailslist, function (dev) {
                        if ($scope.taskid6.length === 0) {
                            $scope.taskid6.push({
                                ISMSBOM_Id: dev.ISMSBOM_Id,
                                ISMSBOM_BOMName: dev.ISMSBOM_BOMName,
                            });
                        }
                        else if ($scope.taskid6.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid6, function (emp) {
                                if (emp.ISMSBOM_Id === dev.ISMSBOM_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid6.push({
                                    ISMSBOM_Id: dev.ISMSBOM_Id,
                                    ISMSBOM_BOMName: dev.ISMSBOM_BOMName,
                                });
                            }
                        }
                    });
                    angular.forEach($scope.taskid6, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.viewservicedetailslist, function (dd) {
                            if (dd.ISMSBOM_Id === ddd.ISMSBOM_Id ) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.viewservicedetailslist = $scope.templist;


                    });








                    //angular.forEach($scope.viewservicedetailslist, function (dev) {
                    //    if ($scope.taskid6.length === 0) {
                    //        $scope.taskid6.push({
                    //            ISMSPS_ID: dev.ISMSPS_ID,
                    //            ISMSBOM_BOMName: dev.ISMSBOM_BOMName,
                    //        });
                    //    }
                    //    else if ($scope.taskid6.length > 0) {
                    //        var intcount = 0;
                    //        angular.forEach($scope.taskid6, function (emp) {
                    //            if (emp.ISMSPS_ID === dev.ISMSPS_ID) {
                    //                intcount += 1;
                    //            }
                    //        });
                    //        if (intcount === 0) {
                    //            $scope.taskid6.push({
                    //                ISMSPS_ID: dev.ISMSPS_ID,
                    //                ISMSBOM_BOMName: dev.ISMSBOM_BOMName,
                    //            });
                    //        }
                    //    }
                    //});
                    //angular.forEach($scope.taskid6, function (ddd) {
                    //    $scope.templist = [];
                    //    angular.forEach($scope.viewservicedetailslist, function (dd) {
                    //        if (dd.ISMSPS_ID === ddd.ISMSBOM_Id) {
                    //            $scope.templist.push(dd);
                    //        }
                    //    });
                    //    ddd.viewservicedetailslist = $scope.templist;

                    //});

                }
                else {
                    //  swal('No Data Found!!!');
                    $scope.viewservicedetailslist = "";
                }
                



                if ($scope.viewservicedetailslist.length > 0) {
                    $('#Viewdeatils').modal('show');
                  
                } else {
                    swal("No Records Found", " ", "error");
                }
            })

        }


        $scope.edit = function (ss) {
            var pageid = {
                "ISMSBOM_Id": ss.ismsboM_Id
            }
            apiService.create("ISM_Sales_BOM/Sales_BOM_edit", pageid).then(function (promise) {
                $scope.ismsmpR_Id = promise.sales_BOM_edits[0].ismsmpR_Id;
                angular.forEach($scope.serviceListtwo, function (ddd) {
                    angular.forEach(promise.sales_BOM_edits, function (dd) {
                        if (dd.ismspS_ID === ddd.ismspS_ID) {
                            ddd.checked = true;
                        }
                    });
                });
                $scope.ismsbom_bomname = promise.sales_BOM_edits[0].ismsboM_BOMName;
                $scope.ismsboM_BOMDescription = promise.sales_BOM_edits[0].ismsboM_BOMDescription;
                $scope.ismsboM_Id = promise.sales_BOM_edits[0].ismsboM_Id;
                

            });
        };

        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted1 = function (field) {
            return $scope.submittedSM;
        };
        $scope.cancelI = function () {
            $state.reload();
        }
       
        $scope.interacted2 = function (field) {
            return $scope.submittedIT;
        };
        $scope.canceBOM = function () {

            $scope.ismsmpR_Id = "";
            $scope.ismsbom_bomname = "";
            $scope.ismsboM_BOMDescription = "";
            $scope.checkall = false;
            $scope.all_check($scope.checkall);
        }
        $scope.interacted3 = function (field) {
            return $scope.submittedBOM;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.serviceListtwo.some(function (options) {
                return options.checked;
            });
        };

    }
})();