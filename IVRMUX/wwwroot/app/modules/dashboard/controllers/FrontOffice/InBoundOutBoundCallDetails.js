(function () {
    'use strict';
    angular
.module('app')
        .controller('InOutBoundCallDetailController', InOutBoundCallDetailController)

    InOutBoundCallDetailController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function InOutBoundCallDetailController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {

        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.page1 = "page1";
        $scope.search = " ";

        $scope.disabledata = false;
        $scope.hmin = new Date();
        $scope.hmin.setHours(0);
        $scope.hmin.setMinutes(0);
        $scope.FOMST_HDWHrMin = $scope.hmin;

        var d = new Date();
        d.setHours(0);
        d.setMinutes(0);
        $scope.min = d;

        var maxsnfttim= new Date();
        maxsnfttim.setHours(18);
        maxsnfttim.setMinutes(0);
        $scope.maxtimme = maxsnfttim;

        var d2 = new Date();
        d2.setHours(7);
        d2.setMinutes(0);
        $scope.max = d2;
       
        $scope.hstep = 1;
        $scope.mstep = 1;
      
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = false;

        //$scope.gridOptions1 = {
        //    enableFiltering: true,
        //    enableColumnMenus: false,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,
        //    columnDefs: [
        //        { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableHiding: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //        { name: 'ismioC_Date', displayName: 'Date', enableHiding: false },
        //        { name: 'ismioC_Name', displayName: 'Name', enableHiding: false },
        //        { name: 'ismioC_From', displayName: 'From Time', enableHiding: false },
        //        { name: 'ismioC_To', displayName: 'To Time', enableHiding: false },
        //        { name: 'ismioC_Purpose', displayName: 'Purpose', enableHiding: false },
        //        { name: 'ismioC_InOutFlag', displayName: 'In/Out', enableHiding: false },
        //        {
        //            field: 'id', name: '',
        //            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
        //                '<div class="grid-action-cell">' +
        //                '<a href="javascript:void(0)" ng-click="grid.appScope.editdata(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' + '</div>'
        //        }
        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    }
        //};
        //grid end

        //validation start 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //popup strart

        //TO clear  popupgrid data
        $scope.clearpopupgrid1 = function () {
            $scope.viewrecordspopupdisplay1 = "";
        };
        //popup end
        //validation end
        /* loading start*/
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 20;
            apiService.getURI("InOutBoundCallDetail/getalldetails", pageid).
                then(function (promise) {
                    $scope.disabledata = true;
                    $scope.gridOptions1 = promise.inoutboundlist;
                    console.log($scope.gridOptions1);
                    //angular.forEach($scope.gridOptions1, function (value, key) {
                    //    var fdate = value.ismioC_Date.split('T');
                    //    value.ismioC_Date = fdate[0];
                    //});
                });
        };
        /* loading end*/
        /* post data start*/

        $scope.OnChangeOT = function (hrmeid, monthvalue) {
            $scope.date = new Date();
            var datevar = $scope.vmsdT_Punchdate == null ? $scope.date : $filter('date')($scope.vmsdT_Punchdate, "yyyy-MM-dd");
            $scope.vmsdT_Punchdate = datevar;
            if (monthvalue == 0) {
                $scope.ivrM_Month_Id = parseInt($filter('date')($scope.date, "MM"));
            }
            var data = {
                "HRME_Id": hrmeid,
                "monthid": monthvalue,
                "VMSDT_Punchdate": $scope.vmsdT_Punchdate
            };
            apiService.create("InOutBoundCallDetail/employeereport", data).
                then(function (promise) {
                    $scope.reportdata = promise.reportdata;
                    if (promise.reportdata.length > 0) {
                        $scope.disabledata = true;
                    }
                });
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var datevar = $scope.ismioC_Date == null ? "" : $filter('date')($scope.ismioC_Date, "yyyy-MM-dd");
                $scope.ismioC_Date = datevar;
                var data = {
                    "ISMIOC_Id": $scope.ismioC_Id,
                    "ISMIOC_Date": $scope.ismioC_Date,
                    "ISMIOC_From": $filter('date')($scope.ismioC_From, "HH:mm"),
                    "ISMIOC_To": $filter('date')($scope.ismioC_To, "HH:mm"),
                    "ISMIOC_Purpose": $scope.ismioC_Purpose,
                    "ISMIOC_Name": $scope.ismioC_Name,
                    "ISMIOC_Remarks": $scope.ismioC_Remarks,
                    "ISMIOC_TransferToName": $scope.ismioC_TransferToName,
                    "ISMIOC_InOutFlag": $scope.ismioC_InOutFlag,
                    "ismioC_CallNo": $scope.ismioC_CallNo
                };
                apiService.create("InOutBoundCallDetail/savedata", data).
                    then(function (promise) {
                        if (promise.returnvalue === "success") {
                            //$scope.gridOptions1.data = promise.filldata;
                            swal('Record Saved/Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnvalue === "Duplicate") {
                            //$scope.gridOptions1.data = promise.filldata;
                            swal('Record already exist.');
                        }
                        else {
                            swal('Record Not Saved/Updated Successfully');
                        }
                    });
            }
                else { $scope.submitted = true; }
            };
        /* post data end*/
        //clearing data start

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("Table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "InOutBoundReport.xls");
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };

        $scope.cleardata = function () {
            $scope.disabledata = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.ismioC_Date = "";
            $scope.ismioC_From = "";
            $scope.ismioC_To = "";
            $scope.ismioC_Purpose = "";
            $scope.ismioC_Name = "";
            $scope.ismioC_Remarks = "";
            $scope.ismioC_TransferToName = "";
            $scope.ismioC_InOutFlag = "";
            $scope.ismioC_CallNo = "";
        };

        $scope.editdata = function (data) {
            var id = data.ismioC_Id;
            apiService.getURI("InOutBoundCallDetail/getdetails", id).then(function (promise) {
                $scope.ismioC_Id = promise.editdata[0].ismioC_Id;
                if (promise.editdata[0].ismioC_Date !== null) {
                    $scope.ismioC_Date = new Date(promise.editdata[0].ismioC_Date);
                }
                else {
                    $scope.ismioC_Date = null;
                }
                $scope.ismioC_From = moment(promise.editdata[0].ismioC_From, 'HH:mm').format();
                $scope.ismioC_To = moment(promise.editdata[0].ismioC_To, 'HH:mm').format();
                $scope.ismioC_Purpose = promise.editdata[0].ismioC_Purpose;
                $scope.ismioC_Name = promise.editdata[0].ismioC_Name;
                $scope.ismioC_Remarks = promise.editdata[0].ismioC_Remarks;
                $scope.ismioC_TransferToName = promise.editdata[0].ismioC_TransferToName;
                $scope.ismioC_InOutFlag = promise.editdata[0].ismioC_InOutFlag;
                $scope.ismioC_CallNo = promise.editdata[0].ismioC_CallNo;
            });
        };
        //edit end

        $scope.deletedata = function (data) {
            var id = data.vmsdT_ID;
            apiService.getURI("InOutBoundCallDetail/deletedata", id).then(function (promise) {
                $scope.vmsdT_ID = promise.editdata[0].vmsdT_ID;
                $scope.hrmE_Id = promise.editdata[0].hrmE_Id;
                $scope.vmsdT_Punchdate = promise.editdata[0].vmsdT_Punchdate;
                $scope.vmsdT_PunchIn = moment(promise.editdata[0].vmsdT_PunchIn, 'HH:mm').format();
                $scope.ivrM_Month_Id = promise.editdata[0].vmsdT_MonthId;
                $scope.vmsdT_PunchOut = moment(promise.editdata[0].vmsdT_PunchOut, 'HH:mm').format();
                $scope.vmsdT_Remarks = promise.editdata[0].vmsdT_Remarks;
            });
        };

        //activate start
        $scope.deactive = function (Data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (Data.fomS_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

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
                        apiService.create("InOutBoundCallDetail/deactivate", Data).
                            then(function (promise) {
                                if (promise.returnvalue === "success") {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.valstrtm = function (timedata) {
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setMinutes(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.ismioC_To = "";
        };

        $scope.validatemax = function (maxdata) {
            if (maxdata >= new Date($scope.ismioC_From)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.ismioC_To = "";
            }
        };

        $scope.maxDate = new Date();
        $scope.filtervalue1 = [];
        //quldatee.getFullYear(),
        //quldatee.getMonth() + 12,
        //quldatee.getDate()
        //);
    }
    
})();   