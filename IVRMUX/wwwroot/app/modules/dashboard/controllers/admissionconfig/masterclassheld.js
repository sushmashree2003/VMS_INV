(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterclassheldController', masterclassheldController)

    masterclassheldController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function masterclassheldController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.checkboxchcked = [];
        $scope.sectioncheckboxchcked = [];
        $scope.noOfDays = [];
        $scope.monthList = [];
        $scope.noOfClassHeldCount = [];
        $scope.position = [];

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.loadData = function () {
            var pageid = 2;
            apiService.getURI("MasterClassHeld/getAllData", pageid).
                then(function (promise) {
                    $scope.displaygrid = false;
                    $scope.allAcademicYear = promise.allyear;
                    $scope.CurrentacdYear = promise.currentYear;
                    for (var i = 0; i < $scope.allAcademicYear.length; i++) {
                        name = $scope.allAcademicYear[i].asmaY_Id;
                        for (var j = 0; j < $scope.CurrentacdYear.length; j++) {
                            if (name == $scope.CurrentacdYear[j].asmaY_Id) {
                                $scope.allAcademicYear[i].Selected = true;
                                $scope.AMAY_Id = $scope.CurrentacdYear[j].asmaY_Id;
                            }
                        }
                    }
                    $scope.asmaYFromDate = $scope.CurrentacdYear[0].asmaY_From_Date;
                    $scope.classDrpDwn = promise.classDrpDwn;
                    $scope.sectionDrpDwn = promise.sectionDrpDwn;
                    $scope.monthlisttemp = promise.monthList;

                    var date = new Date($scope.asmaYFromDate);

                    $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"
                    ];
                    $scope.months = [];
                    $scope.year = [];

                    for (var i = 0; i < 12; i++) {
                        $scope.months.push($scope.monthNames[date.getMonth()]);
                        $scope.year.push(date.getFullYear());
                        // Add a month each time
                        date.setMonth(date.getMonth() + 1);
                    }
                    $scope.monthByOrder = [];
                    for (var i = 0; i < $scope.months.length; i++) {
                        name = $scope.months[i].trim();
                        for (var j = 0; j < promise.monthList.length; j++) {
                            if (name.toLowerCase() == promise.monthList[j].ivrM_Month_Name.toLowerCase().trim()) {
                                $scope.monthByOrder.push(promise.monthList[j]);
                            }
                        }
                    }

                    $scope.monthList = $scope.monthByOrder;
                    //$scope.monthList = promise.monthList;
                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].ascH_ClassHeld = 0; $scope.usercheck = true; $scope.all_check();
                    });
                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].acd_Year = $scope.year[i];
                    });
                })
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.isOptionsRequired = function () {

            return !$scope.sectionDrpDwn.some(function (options) {
                return options.ASMS_Id;
            });
        }
        $scope.onYearChange = function () {

            angular.forEach($scope.allAcademicYear, function (yyy) {

                if (yyy.asmaY_Id == $scope.AMAY_Id) {
                    $scope.asmaYFromDate = yyy.asmaY_From_Date;

                    var date = new Date($scope.asmaYFromDate);

                    $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"]

                    $scope.months = [];
                    $scope.year = [];

                    for (var i = 0; i < 12; i++) {
                        $scope.months.push($scope.monthNames[date.getMonth()]);
                        $scope.year.push(date.getFullYear());
                        // Add a month each time
                        date.setMonth(date.getMonth() + 1);
                    }
                    $scope.monthByOrder = [];
                    for (var i = 0; i < $scope.months.length; i++) {
                        name = $scope.months[i].trim();
                        for (var j = 0; j < $scope.monthlisttemp.length; j++) {
                            if (name.toLowerCase() == $scope.monthlisttemp[j].ivrM_Month_Name.toLowerCase().trim()) {
                                $scope.monthByOrder.push($scope.monthlisttemp[j]);
                            }
                        }
                    }

                    $scope.monthList = $scope.monthByOrder;
                    //$scope.monthList = promise.monthList;
                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].ascH_ClassHeld = 0;
                    });
                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].acd_Year = $scope.year[i];
                    });
                }
            })


            if ($scope.checkboxchcked.length > 0 && $scope.ASMS_Id > 0 && $scope.AMAY_Id > 0 && $scope.ASMCL_Id > 0) {
                $scope.getNoOfClassHeld($scope.position);
            }
        }
        $scope.onclassChange = function () {
            $scope.usercheck = false; $scope.all_check(); $scope.displaygrid = false; $scope.asmC_Id = '';
            if ($scope.checkboxchcked.length > 0 && $scope.AMAY_Id > 0 && $scope.ASMS_Id > 0 && $scope.ASMCL_Id > 0) {
                $scope.getNoOfClassHeld($scope.position);
            }
        }

        $scope.getSelectedSection = function (data) {
            //   $scope.usercheck = true; $scope.all_check();
            $scope.displaygrid = true;
            $scope.ASMS_Id = $scope.asmC_Id;
            //if ($scope.checkboxchcked.length > 0 && $scope.AMAY_Id > 0 && $scope.ASMCL_Id > 0) {
            if ($scope.AMAY_Id > 0 && $scope.ASMCL_Id > 0) {
                $scope.getNoOfClassHeld($scope.position);
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
            //if ($scope.sectioncheckboxchcked.indexOf(data) === -1) {
            //    // $scope.sectioncheckboxchcked.push(data);
            //    

            //}
            //else {
            //    var index = $scope.sectioncheckboxchcked.indexOf(data);
            //    $scope.ASMS_Id = 0;
            //    $scope.sectioncheckboxchcked.splice(index);
            //    // $scope.sectioncheckboxchcked.splice($scope.sectioncheckboxchcked.indexOf(data), 1);
            //}
        }



        $scope.No_Of_ClassHeld = function (classheld) {

            if (classheld != "" && classheld != undefined && classheld != null) {
                $scope.noOfDays.push(classheld);
                return true;
            }
            else {
                var index1 = $scope.noOfDays.indexOf(classheld);
                $scope.noOfDays.splice(index1);
                swal("Please Enter Value For Class Held Column");
                return false;
            }
        }
        $scope.getSelectedData = function (data, position) {

            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
                $scope.position.push(position);
                $scope.getNoOfClassHeld(position);
            }
            else {
                var index = $scope.checkboxchcked.indexOf(data);
                $scope.checkboxchcked.splice(index);
                var index1 = $scope.noOfDays.indexOf(data.ASCH_ClassHeld);
                $scope.noOfDays.splice(index1);
                var index2 = $scope.position.indexOf(data);
                $scope.position.splice(index2);
            }
        }
        $scope.getNoOfClassHeld = function (position) {

            var monthList = $scope.checkboxchcked;
            var listOfClassHeld = {
                selectedmonthList: monthList,
                "AMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("MasterClassHeld/getClassHeld/", listOfClassHeld).
                then(function (promise) {
                    angular.forEach(promise.noOfClassHeldCount, function (value1, i) {
                        if ($scope.monthList[$scope.position[i]].ascH_ClassHeld > 0) {
                            $scope.monthList[$scope.position[i]].ascH_ClassHeld = $scope.monthList[$scope.position[i]].ascH_ClassHeld;
                        }
                        else if (promise.noOfClassHeldCount[i] == 0) {
                            // $scope.monthList[$scope.position[i]].ascH_ClassHeld = user.ascH_ClassHeld;
                        }
                        else {
                            $scope.monthList[$scope.position[i]].ascH_ClassHeld = promise.noOfClassHeldCount[i];
                        }
                    });
                })
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.monthList.some(function (options) {
                return options.Selected;
            });
        }
        $scope.saveclassHeld = function (data) {

            //if ($scope.ASMS_Id > 0 ) {

            //}
            //else {
            //    swal('Please Select Section Checkbox');
            //    return $scope.submitted = false;
            //}
            //if ($scope.checkboxchcked.length == 0) {
            //    swal('Please Check at least one Month checkbox');
            //    return $scope.submitted = false;
            //}
            //if ($scope.noOfDays.length == 0)
            //{
            //    swal('Please Enter value for Class Held Column');
            //    return $scope.submitted = false;
            //}
            //else {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var monthList = $scope.checkboxchcked;
                // var sectionList = $scope.sectioncheckboxchcked;
                var listOfClassHeld = {
                    selectedmonthList: monthList,
                    // selectedSectionList: sectionList,
                    "AMAY_Id": $scope.AMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterClassHeld", listOfClassHeld).
                    then(function (promise) {

                        if (promise.msgcount > 0) {
                            for (var i = 0; i < promise.msgcount; i++) {
                                alert(promise.message[i]);
                            }

                        }
                        if (promise.returnVal == true) {
                            $scope.Clearid();
                            // $scope.preAdmtoAdmStuList = promise.preAdmtoAdmStuList;
                            swal('Data Saved Successfully');
                        }
                        else {
                            swal('Failed to Save/Update Data');
                        }
                    })
            }
            //}
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.CalNoOfDays = function daysInMonth(m, y) { // m is 0 indexed: 0-11
            switch (m) {
                case 2:
                    return (y % 4 == 0 && y % 100) || y % 400 == 0 ? 29 : 28;
                case 9: case 4: case 6: case 11:
                    return 30;
                default:
                    return 31
            }
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.monthList, function (user) {
                user.Selected = toggleStatus;
                $scope.getNoOfClassHeld($scope.monthList.indexOf(user));
            });
        }

    }
})();