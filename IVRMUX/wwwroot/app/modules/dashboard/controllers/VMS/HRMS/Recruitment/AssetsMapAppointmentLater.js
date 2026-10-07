(function () {
    'use strict';
    angular
        .module('app')
        .controller('assetsmapappointmentController', assetsmapappointmentController);

    assetsmapappointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function assetsmapappointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

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
            window.location.reload();
        };



        //for popup Appintmnets 
        $scope.showModal = function (item) {
            $('#Assetsmap').modal('show');  // Show the modal
            $scope.HRME_Id1 = item.HRME_Id;
            // Call the API after the modal is shown

            var data = {
                "HRME_Id": item.HRME_Id
            }

            apiService.create("Appointment/getassetsalldetails", data).then(function (promise) {

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
                                            loca.HRME_LeftFlag = emp.HRME_LeftFlag;
                                            loca.HRME_EmployeeCode = emp.HRME_EmployeeCode;
                                        }
                                    }
                                }
                            });
                        });
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



        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {
              
                $scope.candidatelist = promise.appointmentCandidateList;
               
                $scope.candidatelist = $scope.candidatelist.map(function (cand) {
                    //  AppointmentDate and HRME_DOJ are valid Date objects
                    var appointmentDate = new Date(cand.AppointmentDate);
                    var doj = new Date(cand.HRME_DOJ);

                    // Calculate the difference in time (in milliseconds)
                    var timeDiff = doj - appointmentDate;

                    // Convert the time difference to days (milliseconds in a day: 86400000)
                    var dayDiff = Math.floor(timeDiff / (1000 * 3600 * 24));

                    cand.daysBetween = dayDiff;

                    return cand;
                });

               

            });
        };
        $scope.isExpired = function (doj) {
            var dojDate = new Date(doj);

            var today = new Date();
            today.setHours(0, 0, 0, 0);  

            return dojDate < today; 
        };


        
       




        $scope.getHRMEId = function (employeeId) {
            var candidate = $scope.candidatelist.find(cand => cand.EmployeeId === employeeId);

            return candidate ? candidate.HRME_Id : null;
        };

        //$scope.savedata = function (user) {
        //    var data = {
        //        "HRME_ID": $scope.HRME_Id1,            
        //        "INVMLO_Id": user.INVMLO_Id,     
        //        "HRMF_Id": user.INVLOC_ID       
        //    };

        //    //$scope.location = [];

        //    apiService.create("Appointment/SaveMapDetails", data).then(function (promise) {
        //    }).catch(function (error) {
        //        console.error("Error saving data:", error);
        //    });
        //};

        $scope.savedata = function (user, SweetAlert) {
            var data = {
                "HRME_ID": $scope.HRME_Id1,
                "INVMLO_Id": user.INVMLO_Id,
                "HRMF_Id": user.INVLOC_ID
            };

            // Show SweetAlert confirmation dialog
            swal({
                title: "Are you sure?",
                text: "Do you want to save this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, save it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        // Proceed with API call if user confirms
                        apiService.create("Appointment/SaveMapDetails", data)
                            .then(function (promise) {
                                swal("Success", "Record saved successfully", "success");
                                // You can update any data or reload the grid if needed here
                            }).catch(function (error) {
                                swal("Error", "There was an error saving the record", "error");
                                console.error("Error saving data:", error);
                            });
                    } else {
                        // If user cancels
                        swal("Cancelled", "The save operation was cancelled", "error");
                    }
                });
        };


































    }
})();