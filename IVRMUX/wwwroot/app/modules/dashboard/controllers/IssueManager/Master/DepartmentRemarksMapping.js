(function () {
    'use strict';
    angular.module('app').controller('DepartmentRemarksMappingController', DepartmentRemarksMappingController)

    DepartmentRemarksMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function DepartmentRemarksMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.searchValueU = "";
        $scope.emailsstd = [{ id: 'emailsstd' }];
        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("EmailDepartmentMapping/getloaddata", pageid).then(function (promise) {
                $scope.deptartmentlist = promise.getdepartment;
                $scope.institution = promise.getinstitution;

                if ($scope.institution !== null && $scope.institution.length > 0) {
                    if ($scope.institution.length === 1) {
                        $scope.MI_Id = $scope.institution[0].mI_Id;
                        $scope.disable = true;
                    } else {
                        $scope.disable = false;
                    }
                }
                $scope.newuser = promise.getsavedlist;
            });
        };

        $scope.onchangeinstitution = function () {
            $scope.deptdetials = false;
            $scope.tempadd = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("EmailDepartmentMapping/onchangeinstitution", data).then(function (promise) {
                if (promise !== null) {
                    $scope.deptartmentlist = promise.getdepartment;
                }
            });
        };

        $scope.onchangedept = function () {
            $scope.deptdetials = false;
            $scope.tempadd = [];
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("EmailDepartmentMapping/onchangedepartment", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getinstdeptsavedlist !== null && promise.getinstdeptsavedlist.length > 0) {
                        $scope.getinstdeptsavedlistnew = promise.getinstdeptsavedlist;
                    }
                }
            });
        };

        $scope.checkduplicate = function () {

            var remarks = $scope.remarks;
            var k = 0;
            angular.forEach($scope.getinstdeptsavedlistnew, function (opt) {
                if (opt.ismdR_Remarks === remarks) {
                    k += 1;
                }
            });
            if (k === 0) {
                angular.forEach($scope.tempadd, function (temp) {
                    if (temp.ISMDR_Remarks === remarks) {
                        k += 1;
                    }
                });
            }

            if (k > 0) {
                $scope.remarks = "";
                swal("Record Already Exists");
            }
        };

        $scope.tempadd = [];
        $scope.addnewrow = function () {

            if ($scope.frmmodule.$valid) {

                angular.forEach($scope.institution, function (ins) {
                    if (ins.mI_Id === parseInt($scope.MI_Id)) {
                        $scope.companyname = ins.mI_Name;
                    }
                });

                angular.forEach($scope.deptartmentlist, function (dept) {
                    if (dept.hrmD_Id === parseInt($scope.HRMD_Id)) {
                        $scope.deptname = dept.hrmD_DepartmentName;
                    }
                });
                var remarksid = 0;
                if ($scope.ISMDR_Id > 0) {
                    remarksid = $scope.ISMDR_Id;
                } else {
                    remarksid = 0;
                }
                var flag = false;
                if ($scope.checkoruncheck === true) {
                    flag = true;
                } else {
                    flag = false;
                }

                $scope.tempadd.push({
                    MI_Id: $scope.MI_Id, mI_Name: $scope.companyname, HRMD_Id: $scope.HRMD_Id, hrmD_DepartmentName: $scope.deptname,
                    ISMDR_Remarks: $scope.remarks, ISMDR_Id: remarksid, ISMDR_ReasonFlag: flag
                });
                $scope.remarks = "";
                $scope.deptdetials = true;
                $scope.disable = true;
                $scope.ISMDR_Id = 0;

            } else {
                $scope.submitted = true;
            }
        };

        $scope.edit = function (row, index) {
            $scope.emplt = $scope.tempemplt;
        };

        $scope.delete = function (row, index) {
            for (var x = 0; x < $scope.tempadd.length; x++) {
                if (x === index) {
                    $scope.tempadd.splice(x, 1);
                }
            }
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.savedata = function () {

            if ($scope.tempadd.length > 0) {

                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "MI_Id": $scope.MI_Id,
                    "temp_deptremarks": $scope.tempadd
                };

                apiService.create("EmailDepartmentMapping/saveremarksdata", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.message === "Save") {
                            if (promise.returnval === true) {
                                swal("Record Saved /Updated  Successfully");
                            } else {
                                swal("Failed To Save /Update Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save / Update Record");
                        }
                        $state.reload();
                    }

                });
            }
        };




        //=====================Editrecord....

        $scope.Editdata = function (user) {
            var data = {
                "HRMD_Id": user.hrmD_Id,
                "MI_Id": user.mI_Id,
                "ISMDR_Id": user.ismdR_Id
            };
            apiService.create("EmailDepartmentMapping/Editremarksdata", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditremarksdata !== null && promise.geteditremarksdata.length > 0) {

                        $scope.geteditremarksdatanew = promise.geteditremarksdata;
                        $scope.deptartmentlist = promise.getdepartment;

                        $scope.MI_Id = $scope.geteditremarksdatanew[0].mI_Id;
                        $scope.HRMD_Id = $scope.geteditremarksdatanew[0].hrmD_Id;
                        $scope.remarks = $scope.geteditremarksdatanew[0].ismdR_Remarks;
                        $scope.ISMDR_Id = $scope.geteditremarksdatanew[0].ismdR_Id;
                    }
                }
            });
        };

        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";

            if (deactiveRecord.ismdR_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "Record Deactivated ";
            } else {
                mgs = "Activate";
                confirmmgs = "Record Activated ";
            }
           

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("EmailDepartmentMapping/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + ' Successfully');
                                }
                                else {
                                    swal('Record Not  Activated/Deactivated');
                                }
                            });
                        $state.reload();
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }                    
                });
        };








        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        // add new row
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };

        //adding email id student

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd1' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        };


        $scope.addtopdays = function (user, index) {

            for (var k = 0; k < $scope.emailsstd.length; k++) {
                var roll = user.emaild;
                var arryind = $scope.emailsstd.indexOf($scope.emailsstd[k]);
                console.log(arryind);
                if (parseInt(arryind) !== parseInt(index)) {
                    if ($scope.emailsstd[k].emaild === roll) {
                        swal("Already Exist");
                        $scope.emailsstd[index].emaild = "";
                        break;
                    }
                }
            }
        };


    }
})();

