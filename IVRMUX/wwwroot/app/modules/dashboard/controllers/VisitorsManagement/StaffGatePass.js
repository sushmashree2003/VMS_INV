
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffGatePassController', StaffGatePassController)

    StaffGatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter', 'myFactoryvisitor']
    function StaffGatePassController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter, myFactoryvisitor) {

        $scope.staff_printoption = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";


        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        
        //========================TO  GEt The Values iN Grid
        $scope.BindData = function () {
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }  
            apiService.getURI("StaffGatePass/Getdetails", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
               
                $scope.filldepartment = promise.filldepartment;
                $scope.alldata = promise.alldata;

            })
        };


        //========================================Get department  Data
        $scope.get_department = function () {

            var data = {
                "HRMD_Id": $scope.hrmD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StaffGatePass/getdepchange/", data).then(function (promise) {
                $scope.filldesignation = promise.filldesignation;
            })
        };


        //========================================Get Designation Data
        $scope.get_emp = function () {

            debugger;
            var data = {
                "HRMDES_Id": $scope.hrmdeS_Id,
                "HRMD_Id": $scope.hrmD_Id,
                "MI_Id": $scope.mI_Id,
            }

            apiService.create("StaffGatePass/get_staff1", data).then(function (promise) {

                $scope.emplist = promise.emplist;

            })
        }


        //============================================== TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {

            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "GPHST_Id": $scope.gphsT_Id,
                    "HRMD_Id": $scope.hrmD_Id,
                    "HRMDES_Id": $scope.hrmdeS_Id,
                    "HRME_Id": $scope.hrmE_Id,

                    "GPHST_Remarks": $scope.gphsT_Remarks,
                    "GPHST_DateTime": $scope.gphsT_DateTime,
                    "GPHST_IDCardNo": $scope.gphsT_IDCardNo,
                    "MI_Id": $scope.mI_Id,

                }
                apiService.create("StaffGatePass/saveRecord", data).
                    then(function (promise) {
                        debugger;
                        if (promise.returnval != null && promise.dulicate != null) {
                            if (promise.dulicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.gphsT_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');

                                        $scope.staff_printoption = false;
                                        $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                        $scope.SchollName = promise.institution[0].mI_Name;
                                        $scope.SchollAdd = promise.institution[0].mI_Address1;
                                        $scope.staff_gate_pass = true;
                                        $scope.currentstaffdata = promise.currentstaffdata;
                                        $scope.alldata.length = 0;
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.gphsT_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            //$state.reload();
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

        //=============================================Get Edit Data
        $scope.EditRecord = function (EditRecord) {
            $scope.studentlsitdata = [];
            var data = {
                "GPHST_Id": EditRecord.gphsT_Id,
                "HRMDES_Id": EditRecord.hrmdeS_Id,
                "HRMD_Id": EditRecord.hrmD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StaffGatePass/EditRecord/", data).then(function (promise) {

                if (promise.editlist.length > 0) {

                    $scope.gphsT_Id = promise.editlist[0].gphsT_Id;
                    $scope.mI_Id = promise.editlist[0].mI_Id;                      
                    $scope.gphsT_IDCardNo = promise.editlist[0].gphsT_IDCardNo;
                    $scope.gphsT_Remarks = promise.editlist[0].gphsT_Remarks;
                  
                    $scope.gphsT_DateTime = new Date(promise.editlist[0].gphsT_DateTime);

                    $scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                    $scope.get_department();

                    $scope.hrmdeS_Id = promise.editlist[0].hrmdeS_Id;
                    $scope.emplist = promise.emplist;
                   // $scope.get_emp();
                    $scope.hrmE_Id = promise.editlist[0].hrmE_Id;
                 
                }
                else {
                    swal('No Record Found!');
                }
            })
        };


        //===================================================
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        

        //==============================================Deactivate
        $scope.deactivate = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.gphsT_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
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
                        apiService.create("StaffGatePass/deactive", newuser1).
                            then(function (promise) {
                           
                                    if (promise.returnval == true) {
                                        swal("Record " + mgs + "d Successfully!!!");

                                    }
                                    else {
                                        swal("Record Not " + mgs + "d Successfully!!!");

                                    }
                                    $state.reload();
                                });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }



        //===========================cancel
        $scope.cancel = function () {
            $state.reload();
        }

      
        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

     


    }
})();


