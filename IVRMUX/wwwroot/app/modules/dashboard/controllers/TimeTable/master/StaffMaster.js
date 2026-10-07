
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffMasterController', StaffMasterController)

    StaffMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function StaffMasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                    
              { name: 'staffName', displayName: 'Staff Name' },
            { name: 'ttmsaB_Abbreviation', displayName: 'Staff Abbreviation' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:            
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmsaB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.savestaffabbreviationdata = function () {
          

            if ($scope.myForm.$valid) {

                var data = {
                    "TTMSAB_Id":$scope.TTMSAB_Id,
                    "HRME_Id": $scope.Staff_Id,
                    "TTMSAB_Abbreviation": $scope.StaffAbbreviation, 
                }
                apiService.create("StaffMaster/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Records Already Exist !');
                            $state.reload();
                        }
                        else if (promise.returnval === false)
                        {
                            swal('Data Not Saved !');
                            $state.reload();
                        }
                        $scope.BindData();
                    })
              
                $scope.BindData();
               $scope.clearid();
            }
            else {
                $scope.submitted = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("StaffMaster/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;    
           $scope.albumNameArray1 = [];
           for (var i = 0; i < promise.stafflist.length; i++) {
               if (promise.stafflist[i].firstName != '') {
                   if (promise.stafflist[i].middleName !== null) {
                       if (promise.stafflist[i].lastName !== null) {

                           $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].middleName + ' ' + promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                       }
                       else {
                           $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].middleName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                       }
                   }
                   else {
                       if (promise.stafflist[i].lastName !== null) {

                           $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                       }
                       else {
                           $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName, hrmE_Id: promise.stafflist[i].hrmE_Id });

                       }
                   }
               }
           }
           $scope.stafflist = $scope.albumNameArray1;

           $scope.albumNameArray = [];
           for (var i = 0; i < promise.ttstafflist.length; i++) {
               if (promise.ttstafflist[i].firstName != '') {
                   if (promise.ttstafflist[i].middleName !== null) {
                       if (promise.ttstafflist[i].lastName !== null) {

                           $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName + promise.ttstafflist[i].middleName + promise.ttstafflist[i].lastName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
                       }
                       else {
                           $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName + promise.ttstafflist[i].middleName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
                       }
                   }
                   else {
                       $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
                   }
               }
           }
           $scope.gridOptions.data = $scope.albumNameArray;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            $scope.submitted = false;
            $scope.myForm.$setPristine(); 
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.asmaY_Id = "";
            $scope.TTMSAB_Id = 0;
            $scope.Staff_Id = "";
            $scope.StaffAbbreviation = "";

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmsaB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("StaffMaster/getdetails", pageid).
            then(function (promise) {
                $scope.TTMSAB_Id = promise.sujectslistedit[0].ttmsaB_Id;
                $scope.Staff_Id = promise.sujectslistedit[0].hrmE_Id;
                $scope.StaffAbbreviation = promise.sujectslistedit[0].ttmsaB_Abbreviation;
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsaB_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("StaffMaster/deletepages", pageid).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');                           
                        }
                        else {
                            swal('Record Not Deleted Successfully!');                           
                        }
                        $scope.BindData();
                    })
                    $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                    $scope.BindData();
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        //active switch
        $scope.switch = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmsaB_ActiveFlag === true) {
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

                apiService.create("StaffMaster/deactivate", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }



    }

})();