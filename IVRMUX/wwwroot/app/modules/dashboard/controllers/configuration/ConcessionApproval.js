

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ConcessionApprovalController', ConcessionApprovalController)

    ConcessionApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ConcessionApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.checkboxchcked = [];

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        $scope.loadData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid2= 2;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            var pageid3 = 2;
            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10;
            var pageid4 = 2;
            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10;
            var pageid5 = 2;
            $scope.currentPage5 = 1;
            $scope.itemsPerPage5 = 10;
            apiService.getURI("ConcessionApproval/loaddata", pageid).
                then(function (promise) {
                    $scope.catdrp = promise.fillcategory
                    $scope.studentdrp = promise.fillstudentlst
                    $scope.concessionliststudent = promise.concessionliststudent;
                    $scope.concessionliststaff = promise.concessionliststaff;
                })

            $scope.order = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.cleardata = function () {

            //$scope.FMCC_Id = "";
            //$scope.pasr_id = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            $state.reload();
        }

        $scope.onstudchange = function () {
            $scope.secondgrid = false;
            $scope.firstgrid = false;

            $scope.searcgbtn = false;
            $scope.firstgrid = false;
            $scope.checkbutton = false;
            $scope.secondgrid = false;
            $scope.displaysiblingdet = {};
            $scope.fillstaff = {};
        }

        for (var i = 0; i < configsettings.length; i++) {
            if (configsettings.length > 0) {

                $scope.configurationsettings = configsettings[i];

            }
        }

        $scope.oncatchange = function () {

            var listOfStu = {
                "FMCC_Id": $scope.FMCC_Id,
                configurationsettings: $scope.configurationsettings
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //var catid = $scope.FMCC_Id;
            apiService.create("ConcessionApproval/catchange", listOfStu).
                then(function (promise) {
                    if (promise.fillstudentlst != null && promise.fillstudentlst.length > 0) {
                        $scope.studentdrp = promise.fillstudentlst
                        $scope.checkbutton = false;
                        $scope.searcgbtn = false;
                        $scope.firstgrid = true;
                        $scope.checkbutton = false;
                        $scope.secondgrid = false;
                        $scope.displaysiblingdet = {};
                        $scope.fillstaff = {};
                        $scope.fillstaff = {};
                    }
                    else {
                        swal("No students found!");
                    }
                })
        }

        $scope.submitted = false;
        $scope.searchdata = function () {
            if ($scope.myForm.$valid) {
                $scope.firstgrid = false;

                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/oncheck", listOfStu).
                    then(function (promise) {
                        if (promise.concessiontype != "E") {
                            $scope.firstgrid = true;
                            $scope.checkbutton = false;
                            $scope.secondgrid = false;

                            if (promise.concessiontype == "R") {
                                $scope.rtestudent = true;
                            }

                            $scope.displaysiblingdet = promise.fillstudentlst;
                            if ($scope.displaysiblingdet.length == 0 || $scope.displaysiblingdet == null) {
                                swal('Record not found');
                            }

                        }
                        else if (promise.concessiontype == "E") {
                            $scope.fillstaff = promise.fillstaff;

                        }
                        else {
                            swal('No Records');
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.savedatatrans = [];
        $scope.checkdata = function (displaysiblingdet) {
            $scope.savedatatrans = [];
            var count;
            angular.forEach($scope.displaysiblingdet, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans.push(user);
                    count = 1;
                }
            })

            if (count == 1) {
                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id,
                    "confirmorrejectstatus": 'Check',
                    studentdetails: $scope.savedatatrans
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
                    then(function (promise) {

                        if (promise.fillstudentlst.length > 0) {
                            $scope.labelstatus = true;
                            $scope.secondgrid = true;
                            $scope.checkbutton = true;
                            $scope.searcgbtn = true;
                            $scope.displayselectedlst = promise.fillstudentlst;
                        }
                        else {
                            swal("Records not found!");
                        }
                    })
            }

            else {
                swal('Kindly select any one student');
            }

        }



        $scope.changecheck = function (userselct) {
            angular.forEach($scope.displaysiblingdet, function (user) {
                user.isSelected = false;
            })
            angular.forEach($scope.displaysiblingdet, function (user) {
                if (user.pasrS_Id == userselct.pasrS_Id) {
                    user.isSelected = true;
                }
            })
        }

        $scope.changecheckmatch = function (userselct) {
            angular.forEach($scope.displayselectedlst, function (user) {
                user.isSelected = false;
            })
            angular.forEach($scope.displayselectedlst, function (user) {
                if (user.amsT_Id == userselct.amsT_Id) {
                    user.isSelected = true;
                }
            })
        }

        $scope.savedatatrans1 = [];
        $scope.confirmdata = function (displayselectedlst) {

            var count;
            angular.forEach(displayselectedlst, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans1.push(user);
                    $scope.HRME_Id = user.hrmE_Id;
                    count = 1;
                }
            })


            if (count == 1) {
                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id,
                    "confirmorrejectstatus": 'C',
                    "HRME_Id": $scope.HRME_Id,
                    studentdetails: $scope.savedatatrans1,
                    studentdetails1: $scope.savedatatrans
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Record Confirmed Successfully");
                        }
                        else if (promise.returnval == false) {
                            swal("Record Not Saved Successfully");
                        }

                        $state.reload();

                    })
            }

            else {
                swal('Kindly select any one student');
            }


        }


        $scope.showmodaldetailsimage = function (data) {
            $('#preview').attr('src', data);
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.savedatatrans2 = [];
        $scope.rejectdata = function (displaysiblingdet) {

            angular.forEach(displaysiblingdet, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans2.push(user);
                }
            })

            var listOfStu = {
                "FMCC_Id": $scope.FMCC_Id,
                "PASR_Id": $scope.pasr_id,
                "confirmorrejectstatus": 'R',
                studentdetails: $scope.savedatatrans2,
                studentdetails1: $scope.savedatatrans
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ConcessionApproval/saveconfirmdata", listOfStu).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record Rejected Successfully");
                    }
                    else if (promise.returnval == false) {
                        swal("Record Not Saved Successfully");
                    }

                    $state.reload();
                })
        }

    }
})();
