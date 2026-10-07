

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeMasterTermsController', feetrController)

    feetrController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function feetrController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fmT_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "idforedt";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        $scope.search1 = "";
        $scope.search2 = "";
        $scope.termsdisable = false;

        $scope.termduadatestab = false;
        var name = "";
        $scope.loaddata = function () {
            $scope.termsdisable = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.page3 = "page3";
            var pageid = 2;
            apiService.getURI("FeeMasterTerms/getalldetails", pageid).
        then(function (promise) {
            
            $scope.pages = promise.feetermsarray;
            $scope.arrlist6 = promise.feetermsarrayDrop;
            $scope.arrlist7 = promise.hdnames;
            $scope.arrlistchk = promise.insnames;
            $scope.termsmaping = promise.dataretrive;
            $scope.arrlist9 = promise.feetermsarray;
            $scope.arrlist11 = promise.hdnames;
            $scope.arrlist8 = promise.academicdrp;
            $scope.saveddatadisplay = promise.duadateget;

            $scope.totcountfirst = $scope.pages.length;
            $scope.totcountsecond = $scope.termsmaping.length;

        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }

        }
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.ftI_NAME).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
       
        $scope.cance = function () {
            $scope.fmT_Id = 0;
            $scope.FMT_Name = "";
            $scope.submitted = false;
            $scope.myform1.$setPristine();
            $scope.myform1.$setUntouched();
            $scope.search1 = "";
        }
        $scope.submitted = false;
        $scope.savedata = function () {
            
            if ($scope.myform1.$valid) {
                var data = {

                    "FMT_Id": $scope.fmT_Id,
                    "FMT_Name": $scope.FMT_Name,
                    "FMT_ActiveFlag": true,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeMasterTerms/", data).
                then(function (promise) {
                    if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                        swal('Record Already Exist');

                    }
                    if (promise.returnduplicatestatus == "Duplicate") {
                        swal('Record Already Exist');
                       // $scope.loaddata();
                        $state.reload();

                    }
                    else if (promise.returnduplicatestatus == "Save") {
                       
                        swal('Record Saved Successfully');
                        // $scope.loaddata();
                        $state.reload();
                     
                    }

                    else if (promise.returnduplicatestatus == "NotSave") {
                    
                        swal('Record Not Saved');
                        // $scope.loaddata();
                        $state.reload();
                      
                    }
                    else if (promise.returnduplicatestatus == "Update") {
                        
                        swal('Record Updated Successfully');
                        //$scope.loaddata();
                        $state.reload();
                      
                    }
                    else if (promise.returnvalue == "NotUpdate") {
                       
                        swal('Record Not Updated');
                        //$scope.loaddata();
                        $state.reload();
                    }
                  
                    else {
                        $scope.pages = promise.feetermsarray;

                        if (promise.status != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }

                        //$scope.loaddata();
                        //$scope.cance();
                        $state.reload();
                    }


                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMT_Name": $scope.search,
                "FMT_Name": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterTerms/1", data).
        then(function (promise) {
            $scope.pages = promise.feetermsarray;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmT_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeMasterTerms/deletepages", pageid).
                    then(function (promise) {
                        $scope.pages = promise.feetermsarray;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped', 'Term And Head ');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmT_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeMasterTerms/getdetails", pageid).
            then(function (promise) {
                $scope.FMT_Name = promise.feetermsarray[0].fmT_Name;

                $scope.fmT_Id = promise.feetermsarray[0].fmT_Id;
            })
        }
        $scope.deactive = function (feetermsarray) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var confirmmgs = "";
            if (feetermsarray.fmT_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
            apiService.create("FeeMasterTerms/deactivate", feetermsarray).
            then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record " + confirmmgs + " Successfully");
                }
                else {
                    //swal("Record " + confirmmgs + " Successfully");
                    swal("Record is already been used !!!");
                }
                $scope.loaddata();
            })
            }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
        });
        }

    
        //second tab
        $scope.submitted1 = false;
        $scope.savetermsmap = function (arrlistchk) {
            

            if ($scope.myform2.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })
                $scope.albumNameArrayhd = [];
                angular.forEach($scope.arrlist7, function (role) {
                    if (!!role.selected) $scope.albumNameArrayhd.push(role);
                })
                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Installment From List Atleast One....!')
                }
                else if ($scope.albumNameArrayhd.length === 0) {
                    swal('Please Select Head From List Atleast One....!')
                }
                else {
                    var data = {
                        "FMTFH_Id": $scope.fmtfH_Id,
                        "FMT_Id": $scope.fterm,
                       // "FMH_Id": $scope.hname,
                        "TempararyArrayList": $scope.albumNameArray,
                        "TempararyArrayListhd": $scope.albumNameArrayhd,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeMasterTerms/savedetailY", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus == 'Duplicate' && promise.returnval == false) {
                            swal("Record Already Exist");
                        }
                        else if (promise.returnduplicatestatus === "Save") {
                            swal('Record Saved/Updated Successfully');
                            $scope.termsmaping = promise.dataretrive;
                            $scope.termsdisable = false;

                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "NotSave") {
                            swal('Record Not Saved');
                            $scope.termsmaping = promise.dataretrive;

                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "Update") {
                            swal('Record Saved/Updated Successfully');
                            $scope.termsmaping = promise.dataretrive;
                            $scope.termsdisable = false;

                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "NotUpdate") {
                            swal('Record Not Updated');
                            $scope.termsmaping = promise.dataretrive;

                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        
                        else {
                            swal('Record Not Saved/Updated');
                        }
                    })
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.cance1 = function (arrlistchk, arrlist7) {
            $scope.fmtfH_Id = 0;
            $scope.fterm = "";
            $scope.hname = "";
            for (var i = 0; i < $scope.arrlistchk.length; i++) {
                name = arrlistchk[i].selected
                if (name == true) {
                    arrlistchk[i].selected = false;
                }
            }
            for (var i = 0; i < $scope.arrlist7.length; i++) {
                name = arrlist7[i].selected
                if (name == true) {
                    arrlist7[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myform2.$setPristine();
            $scope.myform2.$setUntouched();
            $scope.search2 = "";
        };


        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "Fee": $scope.search,
                "AcademicYear": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterTerms/1", data).
        then(function (promise) {
            $scope.loaddata();
            $scope.termsmaping = promise.retrivefeeHeadData;

            swal("searched Successfully");
        })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.idforedt;
            var pageid = $scope.editEmployeeY;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeMasterTerms/deletepagesY", pageid).
                    then(function (promise) {
                        $scope.loaddata();
                        $scope.termsmaping = promise.retrivefeeHeadData;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal("Record is already been used !!!");
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueY = function (employee, arrlistchk) {
            $scope.editEmployeeY = employee.idforedt;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeMasterTerms/getdetailsY", pageid).
            then(function (promise) {
                $scope.termsdisable = true;
                $scope.grigview2 = true;
                for (var i = 0; i < $scope.arrlistchk.length; i++) {
                    if ($scope.arrlistchk[i].ftI_ID == promise.dataretrive[0].ftI_Id) {
                        $scope.arrlistchk[i].selected = true
                    }
                    else {
                        $scope.arrlistchk[i].selected = false;
                    }
                }
                $scope.fmtfH_Id = promise.dataretrive[0].fmtfH_Id;
                $scope.fterm = promise.dataretrive[0].fmT_Id;
                $scope.hname = promise.dataretrive[0].fmH_Id;
                angular.forEach($scope.arrlist7, function (pp) {
                    if (pp.fmH_Id == promise.dataretrive[0].fmH_Id) {
                        pp.selected = true;
                    }
                    else {
                        pp.selected = false;
                    }
                })

            })
        }
        //for deactive avtive 
        //$scope.deactiveY = function (retrivefeeHeadData) {
        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    } 
        //    var mgs = "";
        //    var confirmmgs = "";
        //    if (retrivefeeHeadData.fmT_ActiveFlag == 0) {
        //        mgs = "Deactive";
        //        confirmmgs = "Deactivated";
        //    }
        //    else {
        //        mgs = "Active";
        //        confirmmgs = "Activated";
        //    }
        //    swal({
        //        title: "Are you sure",
        //        text: "Do you want to " + mgs + " record??????",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //function (isConfirm) {
        //    if (isConfirm) {
        //    apiService.create("FeeMasterTerms/deactivateY", retrivefeeHeadData).
        //    then(function (promise) {
        //        $scope.loaddata();
        //        $scope.students = promise.retrivefeeHeadData;
        //        if (promise.retrivefeeHeadData.returnval == 0) {
        //            swal("Activated successfully.Kindly contact Administrator");
        //        }
        //        else {
        //            swal("Data Saved/Updated successfully");
        //        } $scope.loaddata();
        //    })
        //} else {
        //    swal("Record " + mgs + " Cancelled");
        //}
        //});
        //}

        //third tab
        $scope.cance2 = function () {
            $scope.termsmapin = "";
            $scope.fmTIdT = "";
            $scope.ASMAYIdT = "";
            $scope.fmHIdT = "";
            $scope.acyrdisabl = false;
            $scope.trmdisabl = false;
            $scope.fhdisabl = false;
            $scope.fmtfhdD_Id = 0;
            $scope.grigview2 = false;
            $scope.submitted2 = false;
            $scope.myform3.$setPristine();
            $scope.myform3.$setUntouched();
            $scope.search3 = "";
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.BindGrid = function (arrlist9, arrlist11) {
            $scope.grigview2 = true;
            var data = {

                "ASMAY_Id": $scope.ASMAYIdT,  // academic year
                "FMT_Id": $scope.fmTIdT,   // term id
                "FMH_Id": $scope.fmHIdT,   // head id
            };
            apiService.create("FeeMasterTerms/Getduedates", data).
                then(function (promise) {
                    if (promise.length > 0) {
                        $scope.termsmapin = promise;
                    }
                    else {
                        swal('Installments are not mapped for your selection ');
                    }

                })

        };
        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(FromDate) > new Date(ToDate)) {
                swal('To Date should be greater than from date');

                return false;
            }
        };
        $scope.submitted2 = false;
        $scope.savedatadues = function (termsmapin) {
            $scope.albumNameArraynewone = [];
            var chkDatecondition = '';
            var fdate = '';
            var tdate = '';

            if ($scope.myform3.$valid) {

                for (var i = 0; i < $scope.termsmapin.length; i++) {
                    var curDate = new Date();
                    fdate = termsmapin[i].fmtfhdD_FromDate;
                    tdate = termsmapin[i].fmtfhdD_ToDate;
                    if (new Date(fdate) > new Date(tdate)) {
                        chkDatecondition = 'NotAllowed';
                    }
                    else {
                        chkDatecondition = 'Allowed';
                    }
                }
                if (chkDatecondition === 'NotAllowed') {
                    swal('To Date should be greater than from date');
                }
                else {
                    for (var i = 0; i < $scope.termsmapin.length; i++) {
                        var f = new Date(termsmapin[i].fmtfhdD_FromDate).toDateString();
                        var t = new Date(termsmapin[i].fmtfhdD_ToDate).toDateString();
                        var a = new Date(termsmapin[i].fmtfhdD_ApplicableDate).toDateString();
                        var d = new Date(termsmapin[i].fmtfhdD_DueDate).toDateString();
                        termsmapin[i].fmtfhdD_FromDate = f;
                        termsmapin[i].fmtfhdD_ToDate = t;
                        termsmapin[i].fmtfhdD_ApplicableDate = a;
                        termsmapin[i].fmtfhdD_DueDate = d;
                        $scope.albumNameArraynewone.push(termsmapin[i]);
                    }
                    var data = {

                        "mainid": $scope.fmtfhdD_Id,
                        "temyrid": $scope.ASMAYIdT,
                        "FMT_Id": $scope.fmTIdT,
                        "FMH_Id": $scope.fmHIdT,
                        "feetfhddd": $scope.albumNameArraynewone,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeMasterTerms/savedetailDDD", data).
                    then(function (promise) {
                        if (promise.returnvalue === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else if (promise.returnvalue === false) {
                            swal('Record Not Saved/Updated Successfully');
                        }
                        else {
                            swal('Record Saved/Updated Successfully');
                            $scope.loaddata();
                            $scope.cance2();
                        }
                    })
                }
            }
            else {
                $scope.submitted2 = true;
            }
        };
        $scope.editEmployeeY = {}
        $scope.deletedataYTTTT = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fmthddid;
            var iiiiii = $scope.editEmployeeY;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeMasterTerms/deletepagesthird", iiiiii).
                    then(function (promise) {
                        $scope.saveddatadisplay = promise.duadateget;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped', 'Term Duadates');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                            $scope.cance2();
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueDYTTTT = function (employee) {
            $scope.editEmployeeY = employee.fmthddid;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeMasterTerms/getdetailsDYthird", pageid).
            then(function (promise) {
                $scope.grigview2 = true;
                $scope.fmtfhdD_Id = promise.arrduadates[0].fmthddid;
                $scope.fmHIdT = promise.arrduadates[0].fmh;
                $scope.fmTIdT = promise.arrduadates[0].fmt;
                $scope.acyrdisabl = true;
                $scope.trmdisabl = true;
                $scope.fhdisabl = true;
                $scope.fmT_Id = promise.arrduadates[0].fmt;
                $scope.fmH_Id = promise.arrduadates[0].fmh;
                $scope.ASMAYIdT = promise.arrduadates[0].asyid;
                $scope.myArrayR = [];
                angular.forEach(promise.arrduadates, function (user) {
                    var frdate = new Date(promise.arrduadates[0].fdate1);
                    user.fmtfhdD_FromDate = frdate;
                    var frtdate = new Date(promise.arrduadates[0].tdate1);
                    user.fmtfhdD_ToDate = frtdate;
                    var fradate = new Date(promise.arrduadates[0].aplc1);
                    user.fmtfhdD_ApplicableDate = fradate;
                    var frddate = new Date(promise.arrduadates[0].ddate1);
                    user.fmtfhdD_DueDate = frddate;
                    user.fmtfH_Id = promise.arrduadates[0].FMTFHNew;
                    user.insname = promise.arrduadates[0].termname;
                    $scope.myArrayR.push(user);
                })
                $scope.termsmapin = $scope.myArrayR;
            })
        }

        $scope.isOptionsRequired = function () {

            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredhd = function () {

            return !$scope.arrlist7.some(function (options) {
                return options.selected;
            });
        }
    }
})();
