(function () {
    'use strict';
    angular.module('app').controller('Rating_MasterController', Rating_MasterController)

    Rating_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function Rating_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

      
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnLoadRatingMasterType", data).then(function (promise) {

                $scope.OverAllRatingGrid = promise.overAllRatingGrid;
                $scope.GetMasterTypeData = promise.overAllTypeGrid;

            });
        };


        

        //$scope.fromtext = function () {
        //    $scope.todisable = false;
        //    var from = 0;
        //    $scope.to = 0; 
        //    if ($scope.from <= 5) {
        //        from = $scope.from;
        //        $scope.fromshow = true;
        //        $scope.to = ++from;
                
        //    } else {
        //        $scope.fromshow = false;
        //        $scope.from = '';
                
        //    }
        //    $scope.todisable = true;

        //};

        $scope.totext = function () {
            $scope.tovalidsshow = true;
            if ($scope.from == 0 ) {
                $scope.tovalidshow = false;
                $scope.to = '';
            }
            else if ($scope.to >= $scope.from) {
                scope.tovalidsshow = true;

            } else if ($scope.to < $scope.from) {
                $scope.tovalidsshow = false;
                $scope.to = '';

            } //else {
            //    $scope.toshow = false;
            //    $scope.to = '';
            //}

        }

        


        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "RATOVERR_FromValue": $scope.from,
                    "RATOVERR_ToValue": $scope.to,
                    "RATOVERR_Rating": $scope.rating,
                    "RATOVERR_Id": $scope.RATOVERR_Id

                };

                apiService.create("Rating_Master/SaveOverAllRating", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }

                    }
                    
                });
                $scope.Loaddata();
                $scope.from = ''
                $scope.to = ''
                $scope.RATOVERR_Id = ''
                $scope.rating = ''
            } else {
                $scope.submitted = true;
            }
            
        };





        $scope.interacted = function (field) {
            return $scope.submitted;
        };

       

        $scope.Clearid = function () {
            $state.reload();
        };


        
        $scope.EditOverAll = function (obj) {
            var data = {
                "RATOVERR_Id": obj.ratoverR_Id,
                "MI_Id": obj.mI_Id
            };

            apiService.create("Rating_Master/OverAllEdit", data).then(function (promise) {
                if (promise !== null) {

                    $scope.from = promise.editDataOverAll[0].ratoverR_FromValue;
                    $scope.to = promise.editDataOverAll[0].ratoverR_ToValue;
                    $scope.RATOVERR_Id = promise.editDataOverAll[0].ratoverR_Id;
                    $scope.rating = promise.editDataOverAll[0].ratoverR_Rating;
                   
                }
            });
        };

        $scope.OverAllActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratoverR_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATOVERR_Id": obj.ratoverR_Id,
                "MI_Id": obj.mI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/OverAllRatingActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.Loaddata();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



    }
})();
