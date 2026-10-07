(function () {
    'use strict';
    angular
.module('app')
.controller('FeeRefundableController', FeeRefundableController)

    FeeRefundableController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeRefundableController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.filterdata1 = "Refunable";
        $scope.sortKey = 'fR_ID';
        $scope.reverse = true;
        $scope.totcountsearch = 0;
        $scope.REF_CheqDate = new Date();
        $scope.cfg = {};
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
              var pageid = 2;
          
            
            apiService.getURI("FeeRefundable/getalldetails", pageid).
            then(function (promise) {
                
                $scope.yearlst = promise.fillyear;
                $scope.currentYear = promise.currentYear;

                $scope.cfg.ASMAY_Id = $scope.currentYear[0].asmaY_Id;


                //for (var i = 0; i < $scope.yearlst.length; i++) {
                //    name = $scope.yearlst[i].asmaY_Id;
                //    for (var j = 0; j < $scope.currentYear.length; j++) {
                //        if (name == $scope.currentYear[j].asmaY_Id) {
                //            $scope.yearlst[i].Selected = true;
                //            $scope.ASMAY_Id = $scope.currentYear[j].asmaY_Id;
                //        }
                //    }
                //}
                if (promise.imN_AutoManualFlag == "Auto")
                {
                    $scope.REF_REC_No = promise.fR_RefundNo;
                }
                
                $scope.classlst = promise.fillclass;
                $scope.sectionlst = promise.fillsection;
                $scope.studentlst = promise.fillstudent;
                $scope.grouplst = promise.fillgroup;
                //if (promise.count > 0)
                //{
                    $scope.thirdgrid = promise.fillthirdgriddata;                  
                    $scope.totcountfirst = promise.fillthirdgriddata.length;
                //}
                //else {
                //    swal("No FEE REFUNDABLE DETAIL Records Found.....!!");
                //}
            
                $scope.gridview1 = false;


                $scope.REF_DATE = new Date();
                $scope.REF_CheqDate = new Date();

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };
        $scope.sort1 = function (keyname1) {
            $scope.sortKey1 = keyname1;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
       // $scope.REF_DATE = new Date();
        //$scope.fromDate = $scope.REF_DATE.getFullYear();
        //$scope.frommon = "";
        //$scope.fromDay = "";
        //$scope.minDatef = new Date(
        //             $scope.fromDate,
        //              $scope.frommon,
        //               $scope.fromDay + 1);

        //$scope.maxDatef = new Date(
        //      $scope.fromDate,
        //       $scope.frommon,
        //        $scope.fromDay + 365);


        $scope.onselectacademic = function (yearlst) {
            $scope.Amst_Id = "";
            var academicyearid = $scope.cfg.ASMAY_Id;
            apiService.getURI("FeeRefundable/getacademicyear", academicyearid).
       then(function (promise) {
           $scope.studentlst = promise.fillstudent;
       })
        };

        $scope.onselectclass = function (classlst) {
            $scope.Amst_Id = "";
            var studid = $scope.Amst_Id;
            apiService.getURI("FeeRefundable/getclasswisestudentlst", studid).
       then(function (promise) {
           $scope.studentlst = promise.fillstudent;
       })
        };

        $scope.OnChangeAcademicYearns = function () {
            
           
            var id = $scope.cfg.ASMAY_Id
            apiService.getURI("FeeRefundable/GetStudentListByYear", id).
          then(function (promise) {
      
            
            if (promise.count > 0) {
                $scope.classlst = promise.fillclass;
            }
            else {
                swal("No Classes Are Mapped");
            }           
        })
        }


        $scope.GetSection = function () {
            
            $scope.Amst_Id = "";
            var data = {
               
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
            }
            apiService.create("FeeRefundable/GetSection", data).
        then(function (promise) {
            

            if (promise.fillsection.length > 0) {
              
                $scope.sectiondrpre = promise.fillsection;
            }
            else {
                $scope.fillsection = {};
                swal('No records found to display');              
            }
        })
        }
        $scope.GetStudent = function () {
            
            $scope.Amst_Id = "";
            var data = {

                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.sectiondrp,


                //"ISMS_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("FeeRefundable/GetStudent", data).
        then(function (promise) {
            

            if (promise.fillstudent.length > 0) {

                $scope.studentlst = promise.fillstudent;
            }
            else {
                $scope.studentlst = {};
                swal('No records found to display');

            }
        })
        }

        $scope.GetStudentListByamst = function () {
            var data = {

                "AMST_ID": $scope.Amst_Id,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "filterrefund": $scope.filterdata1,

                //"ISMS_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("FeeRefundable/GetStudentListByamst", data).
        then(function (promise) {
            
            $scope.studentviewdetails = [];
            if (promise.showstaticticsdetails.length > 0) {
                $scope.studentviewdetails = promise.showstaticticsdetails;
            }
            else {
                $scope.studentviewdetails.length = 0;
            }
            if (promise.fillgroup.length > 0) {

                $scope.grouplst = promise.fillgroup;
            }
            else {
                $scope.fillgroup = {};
                swal('No records found to display');

            }
        })
        }


        $scope.optionToggledGF = function () {
            
            var groupidss;
            for (var i = 0; i < $scope.grouplst.length; i++) {
                
                if ($scope.grouplst[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.grouplst[i].fmG_Id;
                    else
                        groupidss = groupidss + "," + $scope.grouplst[i].fmG_Id;
                }


            }
            if (groupidss != undefined) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "Amst_Id": $scope.Amst_Id,
                    "filterrefund": $scope.filterdata1,
                    "multiplegroupF": groupidss
                }
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeRefundable/onselectgroup", data).
       then(function (promise) {
           if (promise.fillhead.length > 0)
               
           if (promise.fillhead.length > 0) {
               $scope.gridview1 = true;
               $scope.students = promise.fillhead;
               for (var i = 0; i < $scope.students.length; i++) {
                   //$scope.students[i].Amountall = Number($scope.students[i].fsS_RunningExcessAmount) + (Number($scope.students[i].fsS_RefundableAmount) - Number($scope.students[i].fsS_RefundAmount));
                   $scope.students[i].Amountall = Number($scope.students[i].fsS_RunningExcessAmount) + (Number($scope.students[i].fsS_RefundableAmount));
                   //  $scope.students[i].balamt = (Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount)) - Number($scope.students[i].fsS_RefundAmount);
                   $scope.students[i].balamt = (Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount)) ;
                   $scope.students[i].FR_RefundAmount = Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount);
               }
           }
           else {
               $scope.students = {};
               swal("No Records Found.....!!");
           }
           //if (promise.count > 0)
           //{
           //    $scope.gridview1 = true;
           //    $scope.students = promise.fillhead;
           //    for(var i=0;i< $scope.students.length;i++)
           //    {
           //        $scope.students[i].balamt = Number($scope.students[i].fsS_RunningExcessAmount);
           //        $scope.students[i].FR_RefundAmount = $scope.students[i].fsS_RunningExcessAmount;
           //    }
           //}
           //else {
           //    $scope.students = {};
           //    swal("No Records Found.....!!");
           //}
       })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.students.some(function (options) {
                return options.Selected;
            });
        }

        $scope.cleardata = function () {
            //$scope.Amst_Id = "";
            //$scope.FMG_Id = "";
            //$scope.REF_DATE = "";
            //$scope.REF_REMARKS = "";
            //$scope.REF_BANK_CASH = "";
            //$scope.REF_CheqDate = "";
            //$scope.REF_CheqNo = "";
            //$scope.REF_REC_No = "";
            //$scope.REF_BANK_NAME = "";
            //$scope.L_Code = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMAY_Id = "";
            //$scope.gridview1 = false;

            $state.reload();
        }

        $scope.changeacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }

            apiService.create("FeeRefundable/onselectacademicyear", data).
                then(function (promise) {

                    if (promise.fillthirdgriddata.length > 0) {
                        $scope.thirdgrid = promise.fillthirdgriddata;
                        $scope.totcountfirst = promise.fillthirdgriddata.length;
                    }
                    else {
                        swal("No Records Found")
                        $scope.thirdgrid = {};
                    }

                })
        }

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fR_ID;
            var feechequebounceid = $scope.editEmployee;
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
                    apiService.DeleteURI("FeeRefundable/Deletedetails", feechequebounceid).
                   then(function (promise) {
                       if (promise.returnVal == true)
                       {
                           swal("Record Deleted Successfully");
                           $state.reload();
                       }
                       else {
                           swal("Failed to Delete Record");
                       }
                         //  $scope.thirdgrid = promise.fillthirdgriddata;
                   })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });


            //})
        }

        $scope.formbtns = false;
        $scope.onselectmodeofpayment = function () {

            var data = {
                "REF_BANK_CASH": $scope.REF_BANK_CASH,
                "filterinitialdata": $scope.filterdata
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeRefundable/modeofpayment", data).
       then(function (promise) {
         
           if ($scope.REF_BANK_CASH == 'C' || $scope.REF_BANK_CASH == 'B') {
               $scope.formbtns = true;
               $scope.accountlst = promise.fillacclst;
           }
         
       })
        };


        $scope.reF_AMOUNT = true;
        $scope.balanceamt = function (students, index) {
            
            //if (totalgrid[index].checkedgrid == true) {
            //    $scope.Bal_AMOUNT = Number(students[index].REF_AMOUNT) + Number(students[index].enteredamt)
            //}
            //else
            if (Number(students.FR_RefundAmount) > (Number(students.fsS_RunningExcessAmount) + Number(students.fsS_RefundableAmount))) {
                swal("Refund Amount Should not be greater than Excess Amount");
            }
            else {
                $scope.students[index].balamt = (Number(students.fsS_RunningExcessAmount) + Number(students.fsS_RefundableAmount)) ;
                // $scope.students[index].Amountall = Number(students.fsS_RunningExcessAmount) + (Number(students.fsS_RefundableAmount));
                // $scope.students[index].balamt = (Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount)) - Number($scope.students[i].fsS_RefundAmount);
                // $scope.students[index].FR_RefundAmount = Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount);
            }
        }
       
        $scope.edit = function (employee) {
            
            $scope.FRID = employee.fR_ID;
            $scope.disablerec = false;
            $scope.disableflag = false;
           

            apiService.getURI("FeeRefundable/editdetails", $scope.FRID).
            then(function (promise) {

                $scope.gridview1 = true;
                $scope.formbtns = true;
                $scope.ASMAY_Id = promise.fillthirdgriddata[0].asmaY_ID;
                $scope.ASMCL_Id = promise.asmcL_ID;
                $scope.Amst_Id = promise.fillthirdgriddata[0].amsT_ID;
                $scope.FMG_Id = promise.fillthirdgriddata[0].fmG_Id;
                $scope.REF_REC_No = promise.fillthirdgriddata[0].fR_RefundNo;
                $scope.REF_DATE = new Date(promise.fillthirdgriddata[0].fR_Date);
                $scope.REF_BANK_CASH = promise.fillthirdgriddata[0].fR_BANK_CASH;
                $scope.REF_REMARKS = promise.fillthirdgriddata[0].fR_RefundRemarks;
                $scope.REF_CheqDate = new Date(promise.fillthirdgriddata[0].fR_CheqDate);
                $scope.bank.REF_CheqNo = promise.fillthirdgriddata[0].fR_CheqNo;
                $scope.students = promise.editFeeRefund;
                for (var i = 0; i < $scope.students.length; i++) {
                    $scope.students[i].Amountall = Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount);
                    $scope.students[i].balamt = (Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount));
                    $scope.students[i].FR_RefundAmount = Number($scope.students[i].fsS_RunningExcessAmount) + Number($scope.students[i].fsS_RefundableAmount);

                    //$scope.students[i].FR_RefundAmount = $scope.students[i].fsS_RunningExcessAmount;
                    //$scope.students[i].balamt = Number($scope.students[i].fsS_RunningExcessAmount);
                    $scope.students[i].fmH_FeeName = promise.fmH_FeeName;
                    if ($scope.students[i].fsS_RefundOverFlag == true) {
                        $scope.students[i].Selected1 = true;
                        $scope.disablerec = true;
                        $scope.disableflag = true;
                    }
                }
               //  $scope.FR_RefundAmount = promise.fillthirdgriddata[0].reF_AMOUNT;
               // $scope.Bal_AMOUNT = true;
                //$scope.Bal_AMOUNT = Number(promise.fillthirdgriddata[0].reF_AMOUNT) - Number($scope.enteredamt)
               //  $scope.reF_AMOUNT = promise.fillthirdgriddata[0].reF_AMOUNT;
                $scope.bank.REF_BANK_NAME = promise.fillthirdgriddata[0].fR_BankName;
            })
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.selectedStudentList = [];
        $scope.selectedrefundorder = [];
        $scope.bank = {};
        $scope.savedata = function (students) {
            
            if ($scope.myForm.$valid) {

                if ($scope.students != "" && $scope.students != null) {
                    if ($scope.students.length > 0) {
                        for (var i = 0; i < $scope.students.length; i++) {
                            if ($scope.students[i].Selected == true) {
                                if ($scope.students[i].Selected1 == true)
                                {
                                    $scope.selectedrefundorder.push($scope.students[i]);
                                }
                                $scope.selectedStudentList.push($scope.students[i]);
                            }
                        }
                    }
                }
                
               
                var data = {
                    "FR_ID":$scope.FRID,
                    "AMST_ID": $scope.Amst_Id,
                    "ASMAY_ID": $scope.cfg.ASMAY_Id,
                    "ASMCL_ID":$scope.ASMCL_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "FR_Date":new Date($scope.REF_DATE).toDateString(),
                    "FR_RefundRemarks": $scope.REF_REMARKS,
                    "FR_BANK_CASH": $scope.REF_BANK_CASH,
                    "FR_CheqDate":new Date($scope.REF_CheqDate).toDateString(),
                    "FR_CheqNo": $scope.bank.REF_CheqNo,
                    "FR_RefundNo": $scope.REF_REC_No,
                    "FR_BankName": $scope.bank.REF_BANK_NAME,
                    "filterrefund": $scope.filterdata1,
                    savetmpdata: $scope.selectedStudentList,
                    selectedRefundOverList: $scope.selectedrefundorder,
                   
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                if ($scope.fswO_Id > 0) {
                    var disfun = "Update";
                }
                else {
                    var disfun = "Save";
                }

                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + disfun + " Record? ",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false,
                    showLoaderOnConfirm: true,

                },
function (isConfirm) {
    if (isConfirm) {

        apiService.create("FeeRefundable/", data).
                then(function (promise) {
                    //if (promise.returnval == true) {
                    if (promise.returntxt == "Saved" || promise.returntxt == "Updated") {
                        swal('Record ' + promise.returntxt + ' Successfully', 'success');
                        $state.reload();
                        $scope.loaddata();
                    }
                    else {
                        if (promise.returntxt == "not Saved" || promise.returntxt == "not Updated") {
                            swal('Record ' + promise.returntxt + ' Successfully', 'Failed');
                        }
                        else {
                            swal(promise.returntxt);
                        }
                    }

                })
    }
    else {
        swal("Record saved Failed", "Failed");
    }


});






                

            }
            else
            {
                $scope.submitted = true;
            }
         
        };
        //search start

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "2") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;

                }
                else
                if ($scope.search123 == "3") {

                    $scope.txt = false;
                     $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                     $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                 $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "2") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                else if ($scope.search123 == "3") {
                    

                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date
                    }
                }
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeRefundable/searching", data).
            then(function (promise) {
                $scope.thirdgrid = promise.fillthirdgriddata;
                $scope.totcountsearch = promise.fillthirdgriddata.length;
                if (promise.fillthirdgriddata == null || promise.fillthirdgriddata == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }
            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            //$scope.search123 = "";
            //$scope.search_flag = false;
            //$scope.searchtxt = "";
            //$scope.searchnumbr = "";
            //$scope.searchdat = "";
            $state.reload();
            $scope.loaddata();
        }
        //search end
    }

})();