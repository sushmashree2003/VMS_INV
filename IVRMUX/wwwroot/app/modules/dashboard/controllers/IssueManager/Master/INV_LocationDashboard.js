(function () {
    'use strict';
    angular.module('app').controller('INV_LocationDashboardController', INV_LocationDashboardController);
    INV_LocationDashboardController.$inject = ['$compile', '$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$timeout', 'Excel'];
    function INV_LocationDashboardController($compile, $rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout, Excel) {

        $scope.employeedetails = [];
        $scope.employeelist = [];
        $scope.obj = {};
        $scope.HRMDC_ID = 0;
        $scope.url = $location.absUrl();
        $scope.loaddata = function () {

            $scope.floordetails = [];
            $scope.floordetailtemp = [];

            var pageid = 2;
            apiService.getURI("INV_LocationDashboard/getalldetails", pageid).then(function (promise) {

                $scope.floordetailtemp = promise.floordetails;
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
                //$scope.floordetails = promise.floordetails;
                $scope.employeelist = promise.employeedetails;
                $scope.checkoutemployeelist = promise.checkoutemployeelist;
                $scope.locationdetails = promise.locationdetails;
                $scope.inv_locationdashboardsp_list = promise.inv_locationdashboardsp_list;
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

                    //FloorDetails 
                    if ($scope.employeelist !== null && $scope.employeelist.length > 0) {

                        angular.forEach($scope.floordetails, function (loca) {
                            var locaRoomCombined = (loca.Floor_Name && loca.Room_Name) ? (loca.Floor_Name + ' - ' + loca.Room_Name).trim().toLowerCase() : '';
                            var locaRemarks = (loca.INVMLO_LocationRemarks || '').trim().toLowerCase();

                            angular.forEach($scope.employeelist, function (emp) {
                                var empRoom = (emp.INVMLO_LocationRoomName || '').trim().toLowerCase();
                                var isMatch = (loca.INVMLO_Id == emp.INVMLO_Id) ||
                                    (locaRoomCombined !== '' && locaRoomCombined === empRoom) ||
                                    (locaRemarks !== '' && locaRemarks === empRoom);

                                if (isMatch && loca.INVMLO_LocationRemarks != "Common" && emp.HRME_EmployeeCode != "5855" && emp.HRME_EmployeeCode != "25") {
                                    var empName = (emp.EmployeeName || '').trim();
                                    if (empName !== '') {
                                        var hasPhoto = (emp.HRME_Photo != null && emp.HRME_Photo.trim() !== '');
                                        var isActive = (emp.HRME_LeftFlag == 0);
                                        var currentScore = (isActive ? 2 : 0) + (hasPhoto ? 1 : 0);

                                        if (loca.bestScore === undefined || currentScore > loca.bestScore) {
                                            loca.bestScore = currentScore;
                                            loca.HRME_Id = emp.HRME_Id;
                                            loca.EmployeeName = emp.EmployeeName;
                                            loca.HRME_Photo = emp.HRME_Photo;
                                            loca.HRMD_DepartmentName = emp.HRMD_DepartmentName;
                                            loca.HRMDES_DesignationName = emp.HRMDES_DesignationName;
                                            loca.INVMLO_LocationRoomName = emp.INVMLO_LocationRoomName;
                                            loca.INVMLO_Id = emp.INVMLO_Id;
                                            loca.HRME_LeftFlag = emp.HRME_LeftFlag; //HRME_EmployeeCode
                                            loca.HRME_EmployeeCode = emp.HRME_EmployeeCode;
                                        }
                                    }
                                }
                            });
                        });

                    }


                }
            });

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
                    "MI_ID": item.MI_ID || 0,
                    "HRME_Id": item.HRME_Id || 0,
                    "Flag": item.Flag || "",
                    "User_Id": item.INVMLO_Id || 0,
                    "INVMLO_Id": item.INVMLO_Id || 0
                };
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
                        // swal("Location Is Empty !");
                        $scope.employeedetails = [{
                            HRME_Id: item.HRME_Id || 0,
                            EmployeeName: item.EmployeeName || "",
                            HRMDES_DesignationName: item.HRMDES_DesignationName || "",
                            INVMLO_LocationRoomName: item.Room_Name || item.INVMLO_LocationRoomName,
                            INVMLO_Id: item.INVMLO_Id,
                            HRME_Photo: item.HRME_Photo || "",
                            INVMLO_LocationRemarks: item.INVMLO_LocationRemarks || "",
                            RecordDetails: []
                        }];
                        $('#departments').modal('show');
                        $scope.TotalgrossAmountvalue = 0;
                    }

                    // Find the Common location ID for the clicked floor
                    var commonUserId = item.INVMLO_Id;
                    if ($scope.floordetails != null && $scope.floordetails.length > 0) {
                        for (var c = 0; c < $scope.floordetails.length; c++) {
                            if ($scope.floordetails[c].Floor_Name === item.Floor_Name && $scope.floordetails[c].INVMLO_LocationRemarks === 'Common') {
                                commonUserId = $scope.floordetails[c].INVMLO_Id;
                                break;
                            }
                        }
                    }

                    var dataCommon = {
                        "MI_ID": item.MI_ID || item.MI_Id || item.mI_Id || 0,
                        "HRME_Id": item.HRME_Id || item.hrmE_Id || 0,
                        "INVMLO_Id": commonUserId
                    };

                    apiService.create("INV_LocationDashboard/getemployeedetailCommon", dataCommon).then(function (promiseCommon) {
                        $scope.employeedetailsCommon = promiseCommon.employeedetailsCommon || [];
                        $scope.TotalgrossAmountvalueCommon = 0;
                        $scope.TotalEmployeeShareCommon = 0;
                        var totalCommon = 0;
                        var totalEmpShare = 0;
                        if ($scope.employeedetailsCommon != null && $scope.employeedetailsCommon.length > 0) {
                            angular.forEach($scope.employeedetailsCommon, function (prod) {
                                prod.INVMI_ItemName = prod.INVMI_ItemName || prod['Item Name'] || prod['item_name'] || prod.ItemName || '';
                                prod.INVAAT_AssetId = prod.INVAAT_AssetId || prod['Asset-id'] || prod['Asset-Id'] || prod['Asset_id'] || prod.AssetId || '';
                                prod.TotalAmount = prod.TotalAmount !== undefined ? prod.TotalAmount : (prod['Total Amount'] !== undefined ? prod['Total Amount'] : (prod.INVTGRN_PurchaseRate !== undefined ? prod.INVTGRN_PurchaseRate : (prod.Total_Amount !== undefined ? prod.Total_Amount : 0)));
                                prod.MI_Name = prod.MI_Name || prod['Company Name'] || prod['CompanyName'] || prod.CompanyName || prod.company_name || '';
                                prod.INVATCO_CheckOutQty = prod.INVATCO_CheckOutQty !== undefined ? prod.INVATCO_CheckOutQty : (prod['CheckOut-Qty'] !== undefined ? prod['CheckOut-Qty'] : (prod['CheckOut_Qty'] !== undefined ? prod['CheckOut_Qty'] : (prod.CheckOutQty !== undefined ? prod.CheckOutQty : 1)));
                                prod.INVMLO_LocationRoomName = prod.INVMLO_LocationRoomName || prod['Location RoomName'] || prod['LocationRoomName'] || prod.Room_Name || prod.Location_RoomName || '';
                                prod.INVAAT_SerialNo = prod.INVAAT_SerialNo || prod['Serial.No'] || prod['Serial_No'] || prod.SerialNo || prod.serial_no || '';
                                prod.TotalEmployeesOnFloor = prod.TotalEmployeesOnFloor !== undefined ? prod.TotalEmployeesOnFloor : (prod['Total Employees on Floor'] !== undefined ? prod['Total Employees on Floor'] : (prod.TotalEmployees !== undefined ? prod.TotalEmployees : 0));
                                prod.EmployeeShare = prod.EmployeeShare !== undefined ? prod.EmployeeShare : (prod['Employee Share'] !== undefined ? prod['Employee Share'] : (prod.PerPersonCost !== undefined ? prod.PerPersonCost : (prod.Employee_Share !== undefined ? prod.Employee_Share : 0)));
                                prod.PerPersonCost = prod.EmployeeShare;
                                prod.INVATCO_Id = prod.INVATCO_Id || prod['INVATCO_Id'] || prod['invatcO_Id'] || prod['AssetTagCheckOutId'] || 0;
                                prod.INVAAT_FilePath = prod.INVAAT_FilePath || prod['FilePath'] || prod['INVAAT_FilePath'] || prod.FilePath || '';
                                prod.selected = false;

                                var rate = prod.TotalAmount || prod['Total Amount'] || 0;
                                var share = prod.EmployeeShare || prod['Employee Share'] || 0;
                                totalCommon += parseFloat(rate) || 0;
                                totalEmpShare += parseFloat(share) || 0;
                            });
                        }
                        $scope.TotalgrossAmountvalueCommon = (totalCommon % 1 === 0) ? totalCommon : parseFloat(totalCommon.toFixed(2));
                        $scope.TotalEmployeeShareCommon = (totalEmpShare % 1 === 0) ? totalEmpShare : parseFloat(totalEmpShare.toFixed(2));
                    });
                });

            }

        }

        //Check All---
        $scope.checkAll = function (atk) {
            $scope.all = atk;
            var toggleStatus = $scope.all;
            angular.forEach($scope.employeedetails, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        //Harish
        $scope.Remove = function (a, flg) {
            $scope.locationlist = [];

            if (flg === 'ADD') {
                if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                    var selectedEmp = $scope.obj.hrmE_Id1;
                    $scope.employeedetails[0].HRME_Id = selectedEmp.hrmE_Id;
                    $scope.employeedetails[0].EmployeeName = ((selectedEmp.hrmE_EmployeeFirstName || '') + ' ' + (selectedEmp.hrmE_EmployeeMiddleName || '') + ' ' + (selectedEmp.hrmE_EmployeeLastName || '')).replace(/\s+/g, ' ').trim();
                    $scope.employeedetails[0].HRMDES_DesignationName = selectedEmp.hrmE_EmployeeDesignationName || selectedEmp.HRMDES_DesignationName || "";
                    
                    // Pre-fill bottom dropdown
                    $scope.obj.hrmE_Id = selectedEmp;
                }
                return;
            }

            if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                angular.forEach($scope.employeedetails, function (dev) {
                    if (flg == 'REMOVE') {
                        if (dev.HRME_Id === a) {

                            var exists = $scope.locationlist.some(function (loc) {
                                return loc.INVMLO_Id === dev.INVMLO_Id;
                            });

                            if (!exists) {
                                $scope.locationlist.push({
                                    INVMLO_Id: dev.INVMLO_Id,
                                    HRME_Id: dev.HRME_Id,
                                });
                            }
                        }
                    }
                    else {
                        var exists = $scope.locationlist.some(function (loc) {
                            return loc.INVMLO_Id === dev.INVMLO_Id;
                        });

                        if (!exists) {
                            $scope.locationlist.push({
                                INVMLO_Id: dev.INVMLO_Id,
                                HRME_Id: dev.HRME_Id || 0,
                            });
                        }


                    }

                });
            }

            var data = {
                "HRME_Id": a,
                "Flag": flg,
                "locationlistarrays": $scope.locationlist
            };

            apiService.create("INV_LocationDashboard/removelocationemployee", data).then(function (promise) {
                if (promise.message === "Success") {
                    $scope.obj.hrmE_Id1 = null;
                    swal("Record Updated Successfully !");
                    $('#departments').modal('hide');
                }
                $scope.loaddata();
            });
        };

        $scope.optionToggledchk = function () {
            $scope.all = $scope.employeedetails.every(function (options) {
                return options.selected;
            });
        }

        //Check All Common---
        $scope.checkAllCommon = function (atk) {
            $scope.allCommon = atk;
            var toggleStatus = $scope.allCommon;
            angular.forEach($scope.employeedetailsCommon, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.optionToggledchkCommon = function () {
            $scope.allCommon = $scope.employeedetailsCommon.every(function (options) {
                return options.selected;
            });
        }

        $scope.savedata = function () {

            $scope.location = [];
            var unselectedLocation = [];
            var locationlistForRoom = [];
            var employeeIdForRoom = 0;

            if ($scope.obj.hrmE_Id != undefined && $scope.obj.hrmE_Id.hrmE_Id > 0) {
                employeeIdForRoom = $scope.obj.hrmE_Id.hrmE_Id;
            }

            var processedIds = [];

            function processItem(dev) {
                if (!dev) return;
                var coId = dev.INVATCO_Id || dev.invatcO_Id || dev.AssetTagCheckOutId || 0;
                if (!coId || processedIds.indexOf(coId) !== -1) {
                    return;
                }
                processedIds.push(coId);

                var exists = locationlistForRoom.some(function (loc) {
                    return loc.INVMLO_Id === dev.INVMLO_Id;
                });
                if (!exists && dev.INVMLO_Id) {
                    locationlistForRoom.push({
                        INVMLO_Id: dev.INVMLO_Id,
                        HRME_Id: employeeIdForRoom
                    });
                }

                if (employeeIdForRoom > 0) {
                    if (dev.selected === true) {
                        $scope.location.push({
                            INVATCO_Id: coId,
                            HRME_Id: employeeIdForRoom
                        });
                    } else {
                        unselectedLocation.push({
                            INVATCO_Id: coId,
                            HRME_Id: employeeIdForRoom
                        });
                    }
                }
            }

            if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                angular.forEach($scope.employeedetails, function (dev) {
                    if (dev.RecordDetails && dev.RecordDetails.length > 0) {
                        angular.forEach(dev.RecordDetails, function (rec) {
                            processItem(rec);
                        });
                    } else {
                        processItem(dev);
                    }
                });
            }

            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (emp) {
                    if (emp.RecordDetails && emp.RecordDetails.length > 0) {
                        angular.forEach(emp.RecordDetails, function (rec) {
                            processItem(rec);
                        });
                    }
                });
            }

            if ($scope.employeedetailsCommon != null && $scope.employeedetailsCommon.length > 0) {
                angular.forEach($scope.employeedetailsCommon, function (dev) {
                    var coId = dev.INVATCO_Id || dev.invatcO_Id || dev.AssetTagCheckOutId || 0;
                    if (coId && processedIds.indexOf(coId) === -1) {
                        processedIds.push(coId);
                        if (employeeIdForRoom > 0) {
                            if (dev.selected === true) {
                                $scope.location.push({
                                    INVATCO_Id: coId,
                                    HRME_Id: employeeIdForRoom
                                });
                            } else {
                                unselectedLocation.push({
                                    INVATCO_Id: coId,
                                    HRME_Id: employeeIdForRoom
                                });
                            }
                        }
                    }
                });
            }

            if ($scope.location != null && $scope.location.length > 0) {
                var data = {
                    "HRME_Id": employeeIdForRoom,
                    "locationlistarrays": $scope.location,
                    "unselectedlocationlistarrays": unselectedLocation
                };
                apiService.create("INV_LocationDashboard/SaveDetails", data).then(function (promise) {
                    if (promise.flag == "save" || promise.Flag == "save" || promise.returnval == true) {
                        if (locationlistForRoom.length > 0 && employeeIdForRoom > 0) {
                            var dataMap = {
                                "HRME_Id": employeeIdForRoom,
                                "Flag": 'ADD',
                                "locationlistarrays": locationlistForRoom
                            };
                            apiService.create("INV_LocationDashboard/removelocationemployee", dataMap).then(function (promise2) {
                                swal("Record Saved Successfully !");
                                $('#departments').modal('hide');
                                $state.reload();
                            });
                        } else {
                            swal("Record Saved Successfully !");
                            $('#departments').modal('hide');
                            $state.reload();
                        }
                    }
                    else if (promise.flag == "notsave") {
                        swal("Record Not saved !");
                    }
                    else if (promise.flag == "admin") {
                        swal("Please Contact Administrator !");
                    }
                });
            }
            else {
                swal("Select Atleast One CheckBox and Employee !");
            }

        };

        $scope.interacted = function () {
            return $scope.submitted;
        };

        ////OnChangeLocation
        $scope.OnLocationChange = function () {
            $scope.locationdetails = [];

            var data = {
                "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
            };
            apiService.create("INV_LocationDashboard/OnChangeLocation", data).then(function (promise) {
                if (promise.locationdetails !== null && promise.locationdetails.length > 0) {

                    $scope.locationdetails = promise.locationdetails;
                }
            });
        };




        //sushma
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
                $window.open($scope.vbabrsF_FilePath)
            }
        };







    };
})();
