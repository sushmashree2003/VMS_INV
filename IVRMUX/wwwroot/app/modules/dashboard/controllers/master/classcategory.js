(function () {
    'use strict';
    angular
.module('app')
.controller('masterclasscategoryController', masterclasscategoryController)

    masterclasscategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'superCache']
    function masterclasscategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, superCache) {
        //$scope.cls = {};
        //$scope.Selected = [];
        //Date:23-12-2016 for displaying privileges.
        $scope.sortKey = 'asmcC_Id';
        $scope.sortReverse = true;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        $scope.searchValue = "";
        //$scope.clscatId = 0;
        $scope.columnSort = false;
        $scope.isOptionsRequired = function () {
            return !$scope.classDrpDwn.some(function (options) {
                return options.Selected;
            });
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));




        $scope.loadDrpDwn = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("MasterClassCategory").
            then(function (promise) {
                $scope.acdYear = promise.acdYearList;
                $scope.CurrentacdYear = promise.currentYear;
                for (var i = 0; i < $scope.acdYear.length; i++) {
                    name = $scope.acdYear[i].asmaY_Id;
                    for (var j = 0; j < $scope.CurrentacdYear.length; j++) {
                        if (name == $scope.CurrentacdYear[j].asmaY_Id) {
                            $scope.acdYear[i].Selected = true;
                            $scope.ASMAY_Id = $scope.CurrentacdYear[j].asmaY_Id;
                        }
                    }
                }
                
                $scope.categoryDrpDwn = promise.categoryDrpDwn;
                $scope.classDrpDwn = promise.classDrpDwn;
                $scope.sectionDrpDwn = promise.sectionList;
                if (promise.count == 0) {
                    swal('No Records Found');
                }
                else {
                    $scope.classcategoryList = promise.classcategoryList;
                    $scope.presentCountgrid = $scope.classcategoryList.length;
                }
            })
            //$scope.sort = function (keyname) {
            //    $scope.columnSort = true;
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        }
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //$scope.classcheckboxchcked = [];
        //$scope.getSelectedClass = function (data) {

        //    if ($scope.classcheckboxchcked.indexOf(data) === -1) {
        //        $scope.classcheckboxchcked.push(data);
        //    }
        //    else {
        //        $scope.classcheckboxchcked.splice($scope.classcheckboxchcked.indexOf(data), 1);

        //    }
        //}
        $scope.selectedClassList = [];
        $scope.selectedSectionList = [];
        $scope.submitted = false;
        $scope.save = function () {

            if ($scope.myForm.$valid) {

                if ($scope.classDrpDwn != "" && $scope.classDrpDwn != null) {
                    if ($scope.classDrpDwn.length > 0) {
                        for (var i = 0; i < $scope.classDrpDwn.length; i++) {
                            if ($scope.classDrpDwn[i].Selected == true) {
                                $scope.selectedClassList.push($scope.classDrpDwn[i]);
                            }
                        }
                    }
                }
                if ($scope.sectionDrpDwn != "" && $scope.sectionDrpDwn != null) {
                    if ($scope.sectionDrpDwn.length > 0) {
                        for (var i = 0; i < $scope.sectionDrpDwn.length; i++) {
                            if ($scope.sectionDrpDwn[i].Selected1 == true) {
                                $scope.selectedSectionList.push($scope.sectionDrpDwn[i]);
                            }
                        }
                    }
                }


                var data = {
                    // "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMC_Id": $scope.AMC_Id,
                    "ASMCC_Id": $scope.ASMCC_Id,
                    "selectedClass": $scope.selectedClassList,
                    "selectedSection": $scope.selectedSectionList
                    // "ASMS_Id": $scope.ASMS_Id
                }
                apiService.create("MasterClassCategory", data).
                then(function (promise) {
                    if (promise.message != "" && promise.message != null) {
                        alert(promise.message);
                        $scope.clearid();
                    }

                    if (promise.returnVal == true) {
                        if (promise.messagesaveupdate == "Save") {
                            swal('Record Saved Successfully');
                        } else if (promise.messagesaveupdate == "Update") {
                            swal('Record Updated Successfully');
                        }

                        $scope.classcategoryList = promise.classcategoryList;
                        $scope.presentCountgrid = $scope.classcategoryList.length;
                        $scope.clearid();
                    }
                    else if (promise.returnVal == false) {
                        if (promise.messagesaveupdate == "Save") {
                            swal('Record Failed To Save');
                        } else if (promise.messagesaveupdate == "Update") {
                            swal('Record Failed To Update');
                        }

                        $scope.classcategoryList = promise.classcategoryList;
                        $scope.presentCountgrid = $scope.classcategoryList.length;
                        $scope.clearid();
                    }

                })
            }
            else {
                $scope.submitted = true;
            }

        }
        $scope.clearid = function () {
            $state.reload();
            //$scope.ASMAY_Id = "";
            //$scope.AMC_Id = "";
            //$scope.cls.Selected = false;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchByColumn = function (search, columnName) {
            
            if (search != null && search != "" && search != undefined && columnName != null && columnName != "" && columnName != undefined) {
                var data = {
                    "Input": search,
                    "SearchColumn": columnName
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterClassCategory/searchByColumn", data).
               then(function (promise) {
                   if (promise.count == 0) {
                       swal('No Records Found');
                       $scope.searchValue = "";
                   }
                   else {
                       $scope.searchValue = "";
                       $scope.classcategoryList = promise.classcategoryList;
                       $scope.presentCountgrid = $scope.classcategoryList.length;
                   }
               })
            }
            else {
                swal('Please Enter Data In search here... Text Box And Select Column Name From Dropdown. Then Click On Search Icon');
            }
        }

        $scope.edit = function (editdata) {
            
            $scope.edit_flag = true;
            var ids = {
                "ASMCCS_Id": editdata.asmccS_Id,
                "ASMCC_Id": editdata.asmcC_Id
            }
            apiService.create("MasterClassCategory/getdetails", ids).
            then(function (promise) {
                
                $scope.ASMAY_Id = promise.classcategoryList[0].asmaY_Id;
                $scope.AMC_Id = promise.classcategoryList[0].amC_Id;
                $scope.ASMCC_Id = promise.classcategoryList[0].asmcC_Id;
                //  $scope.ASMCL_Id = promise.classcategoryList[0].asmcL_Id;
                //  $scope.ASMS_Id = promise.classcategoryList[0].asmS_Id;

                for (var i = 0; i < $scope.classDrpDwn.length; i++) {
                    if ($scope.classDrpDwn[i].asmcL_Id == promise.classcategoryList[0].asmcL_Id) {
                        $scope.classDrpDwn[i].Selected = true;
                    }
                    else {
                        $scope.classDrpDwn[i].Selected = false;
                    }
                }
                for (var i = 0; i < $scope.sectionDrpDwn.length; i++) {
                    if ($scope.sectionDrpDwn[i].asmS_Id == promise.classcategoryList[0].asmS_Id) {
                        $scope.sectionDrpDwn[i].Selected1 = true;
                    }
                    else {
                        $scope.sectionDrpDwn[i].Selected1 = false;
                    }
                }
                //$("input:checkbox").on('click', function () {
                //    // in the handler, 'this' refers to the box clicked on
                //    var $box = $(this);
                //    if ($box.is(":checked")) {
                //        // the name of the box is retrieved using the .attr() method
                //        // as it is assumed and expected to be immutable
                //        var group = "input:checkbox[name='" + $box.attr("name") + "']";
                //        // the checked state of the group/box on the other hand will change
                //        // and the current value is retrieved using .prop() method
                //        $(group).prop("checked", false);
                //        $box.prop("checked", true);
                //    } else {
                //        $box.prop("checked", false);
                //    }
                //});
            })
        }

        $scope.isOptionsRequired = function () {

            return !$scope.classDrpDwn.some(function (options) {
                return options.Selected;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.sectionDrpDwn.some(function (options) {
                return options.Selected1;
            });
        }

        $scope.deleteRecord = function (asmcC_Id, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterClassCategory/deletedetails", asmcC_Id).
                    then(function (promise) {
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            $state.reload();
                        }
                        if (promise.returnVal == true) {
                            $scope.classcategoryList = promise.classcategoryList;
                            $scope.presentCountgrid = $scope.classcategoryList.length;
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Failed To Delete Record');
                            //$state.reload();
                        }
                    })
                }
                else {
                    swal("Cancelled");
                    //$state.reload();
                }
            });
        }

        $scope.deactive = function (clscatgry, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (clscatgry.is_Active == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To  " + mgs + " Class Category?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterClassCategory/deactivate", clscatgry).
                        then(function (promise) {
                            $scope.classcategoryList = promise.classcategoryList;
                            $scope.presentCountgrid = $scope.classcategoryList.length;
                            if (promise.msgdeactive == "Deactive") {
                                swal("You Can Not Activate/Deactivate This Record. It Is Already Mapped With Student");
                            }
                            else {
                                if (promise.returnVal == true) {
                                    swal('Class Category' + " " + mgs + 'd Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed to Activate/Deactivate Class Category');
                                }
                            }
                        })
                    } else {
                        swal("Cancelled");
                    }
                    $state.reload();
                });
        }

        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.categoryName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();
