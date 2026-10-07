(function () {
    'use strict';
    angular
        .module('app')
        .controller('workingdaysController', workingdaysController)

    workingdaysController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function workingdaysController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //=====================Load--data.............
        $scope.Loaddata = function () {
                        
            var pageid = 2;
            apiService.getURI("workingdays/getdetails", pageid).then(function (promise) {
                $scope.daylist = promise.daylist;
                $scope.alldata = promise.alldata;
                

            })

        }
        //=====================End-----Load--data----//


        //=====================save--record....
        $scope.saverecord = function () {
            $scope.daylistdata = [];
            
            angular.forEach($scope.daylist, function (dy) {
                if (dy.selected == true) {
                    $scope.daylistdata.push({ fomD_Id: dy.fomD_Id });
                }
            });

            if ($scope.myForm.$valid) {

                var data = {
                    "ismmwD_Id": $scope.ismmwD_Id,                    
                    "ISMMWD_Hours": $scope.ISMMWD_Hours, 
                    daylisdata: $scope.daylistdata,
                }
                apiService.create("workingdays/saverecord", data).then(function (promise) {
                    if (promise.returnval != null) {                        
                            if (promise.returnval == true) {
                                if ($scope.ismmwD_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmwD_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }  
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---save--record....


        //=====================Editrecord....
        $scope.EditData = function (user) {
            $scope.editflag = true;
            
            $scope.ismmwD_Id = user.ismmwD_Id;
            apiService.create("workingdays/editrecord", user).then(function (promise) {
                if (promise.editlist.length > 0) {

                    $scope.ismmwD_Id = promise.editlist[0].ismmwD_Id;
                    $scope.ISMMWD_Hours = promise.editlist[0].ismmwD_Hours;
                    $scope.FOMD_Id = promise.editlist[0].fomD_Id;

                    angular.forEach($scope.daylist, function (dd) {
                        var count = 0;
                        angular.forEach(promise.editlist, function (edit) {
                            if (dd.fomD_Id == edit.fomD_Id) {
                                dd.selected = true;
                                count = count+1;
                            }
                            else {
                                dd.selected = false;
                            }
                        })
                        
                    })
                    $scope.totalhours = $scope.ISMMWD_Hours;
                    if ($scope.FOMD_Id > 0) {
                        $scope.count = 1;
                    }
                    
                }
                else {
                    swal("No Records are Found!");
                }

            });

        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            
            $scope.ismmwD_Id = user.ismmwD_Id;

            var dystring = "";
            if (user.ismprtY_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismprtY_ActiveFlag == 0) {
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
                        apiService.create("workingdays/deactiveY", user).
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
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }


        //$scope.searchchkbx = "";
        //$scope.all_check = function () {
        //    var checkStatus = $scope.usercheck;
        //    angular.forEach($scope.houseList, function (itm) {
        //        itm.select = checkStatus;
        //    });
        //}

      
        $scope.selectedDays = function () {
             
            var count = 0;
            angular.forEach($scope.daylist, function (itm) {
                if (itm.selected == true) {
                    count = count + 1;
                }  
            });
            $scope.count = count;
            console.log("Total Days"+$scope.count);

            var hour = $scope.ISMMWD_Hours;
            var hoursss = hour * count;
            if (hoursss > 0) {
                $scope.totalhours = hoursss;
            }
            else {
                $scope.totalhours = 0;
            }
            
            console.log("Total Hours:"+$scope.totalhours);
        }

        $scope.isOptionsRequired = function () {
            return !$scope.daylist.some(function (options) {
                return options.selected;
            });
        }


        $scope.get_totalhours = function () {
            
            var count = 0;
            angular.forEach($scope.daylist, function (itm) {
                if (itm.selected == true) {
                    count = count + 1;
                }
            });         
            var hour = $scope.ISMMWD_Hours; 
            var hoursss = hour * count;
            if (hoursss > 0) {
                $scope.totalhours = hoursss;
            }
            else {
                $scope.totalhours = 0;
            }
           
            console.log("Total Hours:" + $scope.totalhours);
           
        }




    }
})();

