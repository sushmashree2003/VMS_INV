(function () {
    'use strict';
    angular
.module('app')
        .controller('DriverOTController', DriverOTController)

    DriverOTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function DriverOTController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout, superCache) {

        $scope.disabledata = false;
        $scope.ivrM_Month_Id = "";
        $scope.hmin = new Date();
        $scope.hmin.setHours(0);
        $scope.hmin.setMinutes(0);
        $scope.FOMST_HDWHrMin = $scope.hmin;
        $scope.minDatef = new Date();
        $scope.maxDatef = new Date($scope.minDatef.getFullYear(), $scope.minDatef.getMonth(), $scope.minDatef.getDate() - 120);

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

        $scope.validateTomintime = function (timedata) {

            $scope.vmsdT_PunchOut = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
        };

        $scope.validatemax = function (maxdata) {
            var dsttimee = $scope.vmsdT_PunchIn;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.vmsdT_PunchIn)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.vmsdT_PunchOut = "";
            }
        };

        $scope.validatefromtime1 = function (maxdata1) {

            // $scope.vmsdT_PunchIn = maxdata;
            //$scope.vmsdT_PunchOut = "";
            if (maxdata1 >= new Date($scope.vmsdT_PunchIn)) {
                // $scope.totimemax1 = maxdata1;
                //var hh1 = $scope.totimemax1.getHours();
                //var mm1 = $scope.totimemax1.getMinutes();
                //$scope.max1 = maxdata1;

                //$scope.max1.setMinutes(hh);
                //$scope.max1.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLoginTime = "";
            }

            // $scope.vmsdT_PunchIn = "";
        };
        $scope.validateTomintime1 = function (timedata1) {

            $scope.FOMST_IIHalfLoginTime = "";
            $scope.totime1 = timedata1;
            var hh = $scope.totime1.getHours();
            var mm = $scope.totime1.getMinutes();
            $scope.min1 = timedata1;

            $scope.min1.setMinutes(hh);
            $scope.min1.setMinutes(mm);
            $scope.FOMST_IIHalfLoginTime = timedata1;
        };

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SNO', width: '10%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'fomS_ShiftName', width: '30%', displayName: 'Shift Name' },
                 { name: 'vmsdT_PunchIn', displayName: 'Shift Start Time' },
                  { name: 'vmsdT_PunchOut', displayName: 'Shift End Time' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
                 //'<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-red" aria-hidden="true"></i></a>' +
                 // '<span ng-if="row.entity.ttmsaB_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-green" aria-hidden="true"></i></a><span>' +
                 //'</div>'
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   //'</div>'
                    '<div class="grid-action-cell">' +
                     '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup1" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup1(row.entity);"> <i class="fa fa-eye text-purple" ><md-tooltip md-direction="down">View</md-tooltip></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.editdata(row.entity);"> <md-tooltip md-direction="down">Edits</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.fomS_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.fomS_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ]

        };
        //grid end        
        //validation start 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //popup strart
        //TO  View Record
        $scope.viewrecordspopup1 = function (employee, SweetAlert) {            
            $scope.editEmployee = employee.fomsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("DriverOT/getalldetailsviewrecords1", pageid).
                then(function (promise) {
                    $scope.FOMS_ShiftName = promise.filldata[0].fomS_ShiftName;
                    $scope.viewrecordspopupdisplay1 = promise.filldata;
                });
        };

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
            apiService.getURI("DriverOT/getalldetails", pageid).
                then(function (promise) {
                    $scope.disabledata = false;
                    $scope.employeelist = promise.employeelist;
                    $scope.monthlist = promise.monthlist;
                    //$scope.gridOptions1.data = promise.filldata;
                });
        };
        /* loading end*/
        /* post data start*/

        $scope.OnChangeOT = function (hrmeid, monthvalue,type) {
            $scope.date = new Date();
            var datevar = $scope.vmsdT_Punchdate == null ? $scope.date : $filter('date')($scope.vmsdT_Punchdate, "yyyy-MM-dd");
            $scope.vmsdT_Punchdate = datevar;
            if (monthvalue == 0) {
                $scope.ivrM_Month_Id = parseInt($filter('date')($scope.vmsdT_Punchdate, "MM"));
                monthvalue = $scope.ivrM_Month_Id;
            }
            if (type == "Date") {
                $scope.ivrM_Month_Id = parseInt($filter('date')($scope.vmsdT_Punchdate, "MM"));
                monthvalue = $scope.ivrM_Month_Id;
            }
            var data = {
                "HRME_Id": hrmeid,
                "monthid": monthvalue,
                "VMSDT_Punchdate": $scope.vmsdT_Punchdate
            };
            apiService.create("DriverOT/employeereport", data).
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
                var datevar = $scope.vmsdT_Punchdate == null ? "" : $filter('date')($scope.vmsdT_Punchdate, "yyyy-MM-dd");
                $scope.vmsdT_Punchdate = datevar;
                var data = {
                    "VMSDT_ID": $scope.vmsdT_ID,
                    "HRME_Id": $scope.hrmE_Id,
                    "VMSDT_Punchdate": $scope.vmsdT_Punchdate,
                    "VMSDT_PunchIn": $filter('date')($scope.vmsdT_PunchIn, "HH:mm"),
                    "VMSDT_PunchOut": $filter('date')($scope.vmsdT_PunchOut, "HH:mm"),
                    "VMSDT_Remarks": $scope.vmsdT_Remarks,
                    "VMSDT_MonthId": $scope.ivrM_Month_Id,
                    "withpreviousdayflag": $scope.withpreviousdayflag
                };
                apiService.create("DriverOT/savedata", data).
                    then(function (promise) {
                        if (promise.returnvalue === "success") {
                            //$scope.gridOptions1.data = promise.filldata;
                            swal('Record Saved/Updated Successfully');
                            $scope.vmsdT_ID = 0;
                        }
                        else if (promise.returnvalue === "Duplicate") {
                            //$scope.gridOptions1.data = promise.filldata;
                            swal('Record already exist.');
                            $scope.vmsdT_ID = 0;
                        }
                        else if (promise.returnvalue === "Shift") {
                            //$scope.gridOptions1.data = promise.filldata;
                            swal('Shift Not Mapped for this day !!');
                        }
                        else {
                            swal('Record Not Saved/Updated Successfully');
                        }
                    });
                $scope.cleardata();
            }
        else { $scope.submitted = true; }
            };
        /* post data end*/
        //clearing data start

        $scope.exptoex = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Table');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };

        $scope.printData = function () {
            var innerContents = document.getElementById("Table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cleardata = function () {
            $scope.disabledata = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.hrmE_Id = "";
            $scope.vmsdT_PunchIn = "";
            $scope.ivrM_Month_Id = "";
            $scope.vmsdT_PunchOut = "";
            $scope.vmsdT_Remarks = "";
            $scope.withpreviousdayflag = false;
        };

        $scope.editdata = function (data) {
            var id = data.vmsdT_ID;
            apiService.getURI("DriverOT/getdetails", id).then(function (promise) {
                //$scope.disabledata = true;
                $scope.vmsdT_ID = promise.editdata[0].vmsdT_ID;
                $scope.hrmE_Id = promise.editdata[0].hrmE_Id;
                $scope.vmsdT_PunchIn = moment(promise.editdata[0].vmsdT_PunchIn, 'HH:mm').format();
                $scope.ivrM_Month_Id = promise.editdata[0].vmsdT_MonthId;
                $scope.vmsdT_PunchOut = moment(promise.editdata[0].vmsdT_PunchOut, 'HH:mm').format();
                $scope.vmsdT_Remarks = promise.editdata[0].vmsdT_Remarks;
                //$scope.vmsdT_Punchdate = promise.editdata[0].vmsdT_Punchdate;
                if (promise.editdata[0].vmsdT_Punchdate !== null) {
                    $scope.vmsdT_Punchdate = new Date(promise.editdata[0].vmsdT_Punchdate);
                }
                else {
                    $scope.vmsdT_Punchdate = null;
                }

            });
        };
        //edit end

        $scope.deletedata = function (data) {
            var id = data.vmsdT_ID;
            apiService.getURI("DriverOT/deletedata", id).then(function (promise) {
                //$scope.disabledata = true;
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
                        apiService.create("DriverOT/deactivate", Data).
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
    }
    
})();   