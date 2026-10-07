(function () {
    'use strict';
    angular
        .module('app')
        .controller('Checklist_DetailsController', Checklist_DetailsController);

    Checklist_DetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function Checklist_DetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {
        $scope.currentPage = 1;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }


        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;
        $scope.searchValue = "";
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("ChecklistDetailsMapping/getalldetails", pageid).then(function (promise) {
                if (promise.checklisttypeList !== null && promise.checklisttypeList.length > 0) {
                    $scope.checklisttypeList = promise.checklisttypeList;
                }

                if (promise.floorList !== null && promise.floorList.length > 0) {
                    $scope.floorList = promise.floorList;
                }

                if (promise.checklistmappedList !== null && promise.checklistmappedList.length > 0) {
                    $scope.checklistmappedList = promise.checklistmappedList;
                    $scope.presentCountgrid = $scope.checklistmappedList.length;
                    $scope.EmployeeDis = true;
                }

            });
        };

        $scope.cancel = function () {
            // $scope.chklst = "";
            $state.reload();
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            var pageid = item.ismclD_Id;
        
            var data = {
                "ISMCLD_Id": pageid

            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To  Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ChecklistDetailsMapping/DeleteYss", data).
                            then(function (promise) {
                                if (promise.retrunMsg === 'NotRemove') {
                                    swal('Record Not Deleted');


                                }
                                else if (promise.retrunMsg == 'Remove') {
                                    swal('Record Deleted Successfully');
                                }
                                $scope.onLoadGetData();
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  Cancelled!!!");
                    }
                });
        }

        
        $scope.EditData = function (record) {
            var pageid = record.ismclD_Id;

            var data = {

                
                "ISMCLD_Id": pageid

            };
            apiService.create("ChecklistDetailsMapping/editRecord", data).
                then(function (promise) {
                    if (promise.masterdit !== null && promise.masterdit.length > 0) {
                        $scope.ismclT_Id = promise.masterdit[0].ismclT_Id;
                        $scope.ismclD_Id = promise.masterdit[0].ismclD_Id;
                        $scope.hrmF_Id = promise.masterdit[0].hrmF_Id;
                        $scope.ismcL_Name = promise.masterdit[0].ismcL_Name;
                        $scope.ismclD_Qty = promise.masterdit[0].ismclD_Qty;
                    }
                   
                  

                   

                });
        };

        $scope.savejob = function () {

            if ($scope.myForm1.$valid) {


                var data = {

                    "ISMCLT_Id": $scope.ismclT_Id,
                    "ISMCL_Name": $scope.ismcL_Name,
                    "HRMF_Id": $scope.hrmF_Id,
                    "ISMCLD_Qty": $scope.ismclD_Qty,
                    "ISMCLD_Id": $scope.ismclD_Id

                };



                apiService.create("ChecklistDetailsMapping/savedata", data).
                    then(function (promise) {
                        $scope.onLoadGetData();
                       
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                $state.reload();
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Notification") {
                                swal("Notification!");
                                $scope.cancel();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    });
            }
            else { $scope.submitted = true; }
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            //var exportHref = Excel.tableToExcel(tableId, 'Table');
            //$timeout(function () { location.href = exportHref; }, 100);
            //$timeout(function () {
            //    location.href = exportHref;
            //}, 100);

            var divToPrint = document.getElementById("Table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "CheckListDetailsReport.xls");

        };
    }

})();