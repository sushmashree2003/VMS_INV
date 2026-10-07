


(function () {
    'use strict';
    angular
.module('app')
.controller('exammastercategoryController', exammastercategoryController)

    exammastercategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function exammastercategoryController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};
        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SLNO', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'emcA_CategoryName', displayName: 'Category Name' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.emcA_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.emcA_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi1 = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.gridOptions2 = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SLNO', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'asmaY_Year', displayName: 'Academic Year' },
             
              { name: 'emcA_CategoryName', displayName: 'Category Name' },
            { name: 'asmcL_ClassName', displayName: 'Class Name' },
           // { name: 'asmC_SectionName', displayName: 'Section Name' },

                 {
                     field: 'id', name: '',
                     displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                         '<div class="grid-action-cell">' +
                           '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                   '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                //  '<a ng-if="row.entity.ecaC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive2(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                //'<span ng-if="row.entity.ecaC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive2(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                  '</div>'
                 }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi2 = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

            //ng-click="getorgvalue(user,categorylist,classlist)"
            //ng-click="deletedata(user)"
        };

        // TO Save The Data
        $scope.submitted1 = false;
        $scope.savecategoryddata = function () {
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {

                var data = {
                    "EMCA_Id": $scope.EMCA_Id,
                    "EMCA_CategoryName": $scope.EMCA_CategoryName,
                   // "TTMC_ActiveFlag": true,
                }
                apiService.create("exammastercategory/savedetail1", data).
                    then(function (promise) {
                        $scope.gridOptions1.data = promise.mastetr_category_list;
                        $scope.category_list = promise.categorylist;
                        if (promise.returnval === true) {
                            // swal('Data successfully Saved');
                            if (promise.emcA_Id == 0 || promise.emcA_Id < 0) {
                                swal('Record saved successfully');
                            }
                                // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                            else if (promise.emcA_Id > 0) {
                                swal('Record updated successfully');
                            }
                         
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            //  swal('Recards AlReady Exist !');
                            swal('Record already exist');
                        }
                        else {
                            //swal('Data Not Saved !');
                            if (promise.emcA_Id == 0 || promise.emcA_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.emcA_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.loadData();
                        $scope.clear1();
                    })
                //$scope.loadData();
                // $scope.clear1();
            }

        };

        $scope.isOptionsRequired = function () {
           
            return !$scope.class_list.some(function (options) {
                return options.asmcL_Id1;
                });
           
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.section_list.some(function (options) {
                return options.asmS_Id1;
            });

        }


        //$scope.submitted2 = false;
        //$scope.savecategoryclassmap = function () {
        //    
        //    $scope.submitted2 = true;
        //    if ($scope.myForm2.$valid) {
        //        $scope.albumNameArray1 = [];
        //        $scope.albumNameArray2 = [];
        //        angular.forEach($scope.class_list, function (role) {
        //            if (role.asmcL_Id1 === true) {
        //                $scope.albumNameArray1.push(role);
        //            }
        //        })
        //        angular.forEach($scope.section_list, function (role) {
        //            if (role.asmS_Id1 === true) {
        //                $scope.albumNameArray2.push(role.asmS_Id);
        //            }
        //        })
        //        // if ($scope.albumNameArray1.length > 0) {
        //        if ($scope.albumNameArray1.length > 0 && $scope.albumNameArray2.length > 0) {
        //            var data = {
        //                "ASMAY_Id": $scope.ASMAY_Id,
        //                "EMCA_Id": $scope.emcA_Id,
        //                clssids: $scope.albumNameArray1,
        //                secids:$scope.albumNameArray2,
        //            }
        //            apiService.create("exammastercategory/savedetail2", data).
        //                then(function (promise) {
        //                    if (promise.msg === null) {
        //                        if (promise.returnMsg !== "") {
        //                            if (promise.returnMsg === "duplicate") {
        //                                swal('Record Exist already');
        //                                return;

        //                            } else if (promise.returnMsg === "add") {
        //                                swal('Record Saved Successfully', 'success');
        //                                $scope.detailslist = promise.detailslist;
        //                                //$state.reload();


        //                            } else if (promise.returnMsg === "update") {
        //                                swal('Record Updated Successfully', 'success');
        //                                $scope.detailslist = promise.detailslist;
        //                                // $state.reload();
        //                            }
        //                        } else {
        //                            swal('Record Not Saved/Updated Successfully', 'Failed');
        //                        }
        //                        $scope.loadData();
        //                        $scope.clear2();
        //                        $scope.gridOptions2.data = promise.detailslist;
        //                    }
        //                    else {
        //                        if (promise.returnMsg === "add") {
        //                            swal('Record Saved Successfully..But ' + promise.msg, 'success');
        //                            $scope.detailslist = promise.detailslist;
        //                            //$state.reload();


        //                        } else if (promise.returnMsg === "update") {
        //                            swal('Record Updated Successfully..But ' + promise.msg, 'success');
        //                            $scope.detailslist = promise.detailslist;
        //                            // $state.reload();
        //                        }
        //                        else if (promise.returnMsg === "" || promise.returnMsg === 'null' || promise.returnMsg === null) {
        //                            swal(promise.msg, 'Failed');
        //                            $scope.detailslist = promise.detailslist;
        //                            // $state.reload();
        //                        }


        //                    }
        //                    $scope.loadData();
        //                    $scope.clear2();
        //                })
        //        }
        //        else {
        //            // swal("Sorry Can't Save Without Selection of Class");
        //            swal("Sorry Can't Save Without Selection of Class & Section");
        //            $scope.loadData();
        //            $scope.clear2();
        //            $scope.gridOptions2.data = promise.detailslist;
        //        }
        //    }
        //    else {
        //        //wal('At least select one class..!', 'Failed');
        //        $scope.submitted2 = true;
        //    }



        //};
        $scope.savecategoryclassmap = function () {
            var data = {
                "ECAC_Id": $scope.ECAC_Id,
                selected_temp:$scope.selected_temp,
            }
            apiService.create("exammastercategory/savedetail2", data).
                            then(function (promise) {
                                $scope.gridOptions2.data = promise.category_class_list;
                                // $scope.category_list = promise.categorylist;
                                if (promise.returnval === true) {
                                    //swal('Data successfully Saved');
                                    if (promise.ecaC_Id == 0 || promise.ecaC_Id < 0) {
                                        swal('Record saved successfully');
                                    }
                                        // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                    else if (promise.ecaC_Id > 0) {
                                        swal('Record updated successfully');
                                    }


                                }
                                else if (promise.returnduplicatestatus === 'Duplicate') {
                                    // swal('Recards AlReady Exist !');
                                    swal('Record already exist');
                                }
                                else {
                                    if (promise.ecaC_Id == 0 || promise.ecaC_Id < 0) {
                                        swal('Failed to save, please contact administrator');
                                    }
                                    else if (promise.ecaC_Id > 0) {
                                        swal('Failed to update, please contact administrator');
                                    }

                                }
                                $scope.loadData();
                                $scope.clear2();
                            })
        }





        //TO  GEt The Values iN Grid
        $scope.loadData = function () {
            apiService.getDATA("exammastercategory/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.year_list = promise.yearlist;
           $scope.class_list = promise.classlist;
           $scope.section_list = promise.sectionlist;
           $scope.temp_class_list = promise.classlist;
           $scope.temp_section_list = promise.sectionlist;
           $scope.category_list = promise.categorylist;
           $scope.gridOptions1.data = promise.mastetr_category_list;
           $scope.gridOptions2.data = promise.category_class_list;
           $scope.ASMAY_Id = promise.asmaY_Id;
           $scope.temp_year_id = promise.asmaY_Id;
           angular.forEach($scope.year_list, function (opq) {
               if (opq.asmaY_Id == $scope.ASMAY_Id) {
                   opq.Selected = true;
               }
           })
           $scope.get_cate_class();
       })
        };
        //TO clear  data
        $scope.clear1 = function () {
            $scope.EMCA_CategoryName = "";
            $scope.EMCA_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.gridApi1.grid.clearAllFilters();
            $scope.search = "";


        };
        $scope.clear2 = function () {
            $scope.ECAC_Id = 0;
            //$scope.ASMAY_Id = "";
            // $scope.class_list = $scope.temp_class_list;
            $scope.ASMAY_Id =$scope.temp_year_id;
            
            angular.forEach($scope.year_list, function (opq) {
                if (opq.asmaY_Id == $scope.ASMAY_Id) {
                    opq.Selected = true;
                }
            })
            $scope.get_cate_class();
            $scope.section_list = $scope.temp_section_list;
            //angular.forEach($scope.class_list, function (itm1) {
            //    itm1.asmcL_Id1 = false;
            //})
            angular.forEach($scope.section_list, function (itm1) {
                itm1.asmS_Id1 = false;
            })
            $scope.emcA_Id = "";
            $scope.ASMCL_Id = "";
            $scope.selected_temp = [];
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.gridApi2.grid.clearAllFilters();
            $scope.search = "";


        };
        //TO clear  data
        $scope.clearCategory = function () {
            $scope.submitted = false;
            $scope.emcA_Id = "";

        };
        //TO  Edit Record
        $scope.getorgvalue1 = function (employee) {
            $scope.editEmployee = employee.emcA_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("exammastercategory/getdetails1", pageid).
            then(function (promise) {
                $scope.EMCA_CategoryName = promise.edit_m_category[0].emcA_CategoryName;
                $scope.EMCA_Id = promise.edit_m_category[0].emcA_Id;
            })
        }
        var cls_Id = "";
        $scope.getorgvalue2 = function (employee) {
            //$scope.editEmployee = employee.ecaC_Id;
            //var pageid = $scope.editEmployee;
            $scope.clear2();
            apiService.create("exammastercategory/getdetails2", employee).
            then(function (promise) {
                
              //  $scope.EMCA_CategoryName = promise.edit_category_class[0].emcA_CategoryName;
                $scope.ECAC_Id = promise.edit_category_class[0].ecaC_Id;
                $scope.emcA_Id = promise.edit_category_class[0].emcA_Id;
                $scope.ASMAY_Id = promise.edit_category_class[0].asmaY_Id;
                $scope.ASMCL_Id = promise.edit_category_class[0].asmcL_Id;
                cls_Id = promise.edit_category_class[0].asmcL_Id;
                $scope.get_cate_class();
                //$scope.get_cls_sections();
                $scope.edited_secs = [];
                angular.forEach(promise.edit_category_class, function (opq) {
                    angular.forEach($scope.temp_section_list, function (pq) {
                        if(pq.asmS_Id==opq.asmS_Id)
                        {
                            $scope.edited_secs.push(pq);
                        }
                    })
                    
                })
                var ASMAY_Year = "";
                var EMCA_CategoryName = "";
                var ASMCL_ClassName = "";
                angular.forEach($scope.year_list, function (stu) {
                    if (stu.asmaY_Id == $scope.ASMAY_Id) {
                        ASMAY_Year = stu.asmaY_Year;
                    }
                })
                angular.forEach($scope.category_list, function (stu1) {
                    if (stu1.emcA_Id == $scope.emcA_Id) {
                        EMCA_CategoryName = stu1.emcA_CategoryName;
                    }
                })
                angular.forEach($scope.temp_class_list, function (stu2) {
                    if (stu2.asmcL_Id == $scope.ASMCL_Id) {
                        ASMCL_ClassName = stu2.asmcL_ClassName;
                    }
                })
                //angular.forEach($scope.temp_section_list, function (stu3) {
                //    angular.forEach($scope.edited_secs, function (stu4) {
                //        if (stu4.asmS_Id == stu3.asmS_Id) {
                           
                //        }
                //    })
                   
                //})
                var selected_sections = $scope.edited_secs;

                if ($scope.selected_temp.length == 0) {
                    $scope.selected_temp.push({ ASMAY_Id: $scope.ASMAY_Id, ASMAY_Year: ASMAY_Year, EMCA_Id: $scope.emcA_Id, EMCA_CategoryName: EMCA_CategoryName, ASMCL_Id: $scope.ASMCL_Id, ASMCL_ClassName: ASMCL_ClassName, sections: selected_sections });
                }
                else if ($scope.selected_temp.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.selected_temp, function (opq) {

                        if (opq.ASMAY_Id == $scope.ASMAY_Id && opq.EMCA_Id == $scope.emcA_Id && opq.ASMCL_Id == $scope.ASMCL_Id) {
                            opq.sections = selected_sections;
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 0) {
                        $scope.selected_temp.push({ ASMAY_Id: $scope.ASMAY_Id, ASMAY_Year: ASMAY_Year, EMCA_Id: $scope.emcA_Id, EMCA_CategoryName: EMCA_CategoryName, ASMCL_Id: $scope.ASMCL_Id, ASMCL_ClassName: ASMCL_ClassName, sections: selected_sections });
                    }
                }
               // $scope.clear_temp();

            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.emcA_Id;
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
                    apiService.DeleteURI("exammastercategory/deletepages", pageid).
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
                }
            });
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };

        $scope.deactive1 = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.emcA_ActiveFlag === true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                //mgs = "Active";
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",//it!
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("exammastercategory/deactivate1", employee).
                then(function (promise) {
                    if (promise.already_cnt == true)
                    {
                        swal("You Can Not Deactivate This Record,It Has Dependency");
                    }
                    else {
                        if (promise.returnval == true) {
                            swal("Record "+confirmmgs + " " + "successfully");
                        }
                        else {
                            // swal(confirmmgs + " " + " successfully");
                            swal("Record " + mgs + " Failed");
                        }
                    }
                    $scope.clear1();
                    $scope.loadData();

                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.deactive2 = function (employee, SweetAlert) {
           // $scope.clear2();
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ecaC_ActiveFlag === true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
               // mgs = "Active";
                mgs = "Activate";
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

                apiService.create("exammastercategory/deactivate2", employee).
                then(function (promise) {
                    if (promise.already_cnt == true) {
                        swal("You Can Not Deactivate This Record,It Has Dependency");
                    }
                    else {
                        if (promise.returnval == true) {
                            swal("Record " + confirmmgs + " " + "successfully");
                        }
                        else {
                            // swal(confirmmgs + " " + " successfully");
                            swal("Record " + mgs + " Failed");
                        }
                    }
                    $scope.clear2();
                    $scope.loadData();
                    $scope.viewrecordspopup(employee);
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        var name = "";
        var name1 = "";
        $scope.get_cate_class = function () {
            
            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined) {
                //$scope.editEmployee = employee;
                //var pageid = $scope.editEmployee;

                //$scope.ttcC_Id = employee;

                //for (var i = 0; i < $scope.class_list.length; i++) {
                //    name1 = $scope.class_list[i].asmcL_Id1
                //    if (name1 == true) {
                //        $scope.class_list[i].asmcL_Id1 = '';
                //    }
                //}

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                   // "EMCA_Id": pageid
                }

                // apiService.getURI("CategoryClassMapp/getdetails", pageid).

                apiService.create("exammastercategory/get_cate_class", data).
                then(function (promise) {

                    // $scope.ttmC_Id = promise.binddetails[0].ttmC_Id;
                    // $scope.ASMAY_Id = promise.binddetails[0].asmaY_Id;
                    $scope.class_list = promise.classlist;

                    if($scope.ASMAY_Id!="" && $scope.ASMAY_Id!=undefined && $scope.ASMAY_Id!=null)
                    {
                        
                        if ($scope.selected_temp.length > 0)
                        {
                          //  angular.forEach($scope.selected_temp,function(opq)
                          //  {
                               // if(opq.ASMAY_Id==$scope.ASMAY_Id && opq.EMCA_Id==$scope.emcA_Id)
                            //  {

                            var temp_list_cls = $scope.class_list;
                            if ($scope.ECAC_Id != null && $scope.ECAC_Id != "" && $scope.ECAC_Id != undefined && $scope.ECAC_Id != 0)
                            {

                                var alr_cls_cnt = 0;
                                angular.forEach(temp_list_cls, function (ts) {
                                    if(ts.asmcL_Id==$scope.ASMCL_Id)
                                    {
                                        alr_cls_cnt += 1;
                                    }
                                })
                                if (alr_cls_cnt == 0)
                                {
                                    angular.forEach($scope.temp_class_list, function (opq1) {
                                        if (opq1.asmcL_Id == $scope.ASMCL_Id) {
                                            temp_list_cls.push(opq1);
                                        }
                                    })
                                }
                             
                            }
                            $scope.class_list = temp_list_cls;
                                    angular.forEach($scope.class_list,function(opq1)
                                    {
                                        if(opq1.asmcL_Id==$scope.ASMCL_Id)
                                        {
                                            opq1.Selected = true;
                                        }
                                    })
                                    $scope.get_cls_sections();
                               // }
                           // })
                          //  $scope.get_cls_sections();
                        }
                       
                    }



                    //$scope.checkboxchckedcls = [];

                    //for (var i = 0; i < $scope.class_list.length; i++) {
                    //    name1 = $scope.class_list[i].asmcL_Id
                    //    for (var j = 0; j < promise.binddetails.length; j++) {
                    //        if (name1 == promise.binddetails[j].asmcL_Id) {
                    //            $scope.class_list[i].asmcL_Id1 = true;
                    //            $scope.checkboxchckedcls.push(promise.binddetails[j]);
                    //        }
                    //    }
                    //}
                    //var selected_cls_cnt=0;
                    //angular.forEach($scope.class_list, function (cs) {
                    //    if(cs.asmcL_Id1==true)
                    //    {
                    //        selected_cls_cnt += 1;
                    //    }
                    //})
                    //if (selected_cls_cnt > 0)
                    //{
                    //    $scope.get_cls_sections();
                    //}
                   
                    // $scope.asmcL_Id = promise.binddetails[0].asmcL_Id;

                })
            }
            else {
                swal("Please Select Academic Year First !!!");
                $scope.emcA_Id = "";
            }
        }
        $scope.get_cls_sections = function () {
            
            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.ASMAY_Id != null && $scope.emcA_Id != "" && $scope.emcA_Id != undefined && $scope.emcA_Id != null) {
                //  if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.ASMAY_Id!=null) {
                //$scope.selectedclasses = [];
                //$scope.ttcC_Id = employee;

                //angular.forEach($scope.class_list, function (itm) {
                //    if(itm.asmcL_Id1==true)
                //    {
                //        $scope.selectedclasses.push(itm);
                //    }
                //})
               // if ($scope.selectedclasses.length > 0)
              //  {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id":$scope.ASMCL_Id,
                        "EMCA_Id": $scope.emcA_Id,
                       // clssids: $scope.selectedclasses,
                    }

                    // apiService.getURI("CategoryClassMapp/getdetails", pageid).

                    apiService.create("exammastercategory/get_cls_sections", data).
                    then(function (promise) {
                        
                        // $scope.ttmC_Id = promise.binddetails[0].ttmC_Id;
                        // $scope.ASMAY_Id = promise.binddetails[0].asmaY_Id;
                        $scope.section_list = promise.avail_sections;
                        if ($scope.selected_temp.length > 0)
                        {

                            var temp_list_sec = $scope.section_list;
                            if ($scope.ECAC_Id != null && $scope.ECAC_Id != "" && $scope.ECAC_Id != undefined && $scope.ECAC_Id != 0) {
                                if (cls_Id == $scope.ASMCL_Id) {
                                if (promise.sectionlist != null && promise.sectionlist != "")
                                {
                                  
                                    angular.forEach(promise.sectionlist, function (opq1) {
                                        angular.forEach($scope.edited_secs, function (st) {
                                            if (st.asmS_Id == opq1.asmS_Id) {
                                                temp_list_sec.push(st);
                                            }
                                        })

                                    })
                                }
                                }
                               
                            }


                            angular.forEach($scope.selected_temp, function (opq) {

                                if (opq.ASMAY_Id == $scope.ASMAY_Id && opq.EMCA_Id == $scope.emcA_Id && opq.ASMCL_Id == $scope.ASMCL_Id) {
                                    angular.forEach(opq.sections, function (opq1) {
                                        angular.forEach($scope.section_list, function (opq2) {
                                            if(opq2.asmS_Id==opq1.asmS_Id)
                                            {
                                                opq2.asmS_Id1 = true;
                                            }
                                        })
                                    })
                                }
                            })
                           

                        }


                        //angular.forEach($scope.section_list, function (ore) {
                        //    var sec_cnt = 0;
                        //    angular.forEach(promise.selected_sections, function (ire) {
                        //        if (ore.asmS_Id == ire) {
                        //            sec_cnt += 1;
                        //        }

                        //    })
                        //    if (sec_cnt > 0) {
                        //        ore.asmS_Id1 = true;
                        //    }
                        //})


                        //$scope.checkboxchckedcls = [];

                        //for (var i = 0; i < $scope.class_list.length; i++) {
                        //    name1 = $scope.class_list[i].asmcL_Id
                        //    for (var j = 0; j < promise.binddetails.length; j++) {
                        //        if (name1 == promise.binddetails[j].asmcL_Id) {
                        //            $scope.class_list[i].asmcL_Id1 = true;
                        //            $scope.checkboxchckedcls.push(promise.binddetails[j]);
                        //        }
                        //    }
                        //}

                        // $scope.asmcL_Id = promise.binddetails[0].asmcL_Id;

                        if(promise.avail_sections==null || promise.avail_sections==""){
                            swal("All Classes And Sections Are Mapped To Categories");
                            $scope.ASMCL_Id="";
                        }

                        if (promise.sectionlist == null || promise.sectionlist == "") {
                            swal("Selected Classes Doesn't Mapped With Sections");
                            $scope.ASMCL_Id="";
                        }

                    })
                //}
                //else {
                //    // $scope.section_list = "";
                //    $scope.section_list = $scope.temp_section_list;
                //    angular.forEach($scope.section_list, function (imt) {
                //        imt.asmS_Id1 = false;
                //    })
                //}

               
            }
            else {
                     swal("Please Select Academic Year And Category First !!!");
                   // swal("Please Select Academic Year  First !!!");
                     $scope.emcA_Id = "";
                     $scope.ASMCL_Id = "";
                //angular.forEach($scope.class_list, function (imt) {
                //    imt.asmcL_Id1 = false;
                //})
            }
        }

        $scope.selected_temp = [];
        $scope.submitted2 = false;
        //$scope.selectedclass.length == 0
        $scope.save_temp = function () {
            var selected_sections=[];
            var ASMAY_Year = "";
            var EMCA_CategoryName = "";
            var ASMCL_ClassName = "";
            if ($scope.myForm2.$valid) {

                
                angular.forEach($scope.year_list, function (stu) {
                    if (stu.asmaY_Id==$scope.ASMAY_Id) {
                         ASMAY_Year = stu.asmaY_Year;
                    }
                })
                angular.forEach($scope.category_list, function (stu1) {
                    if (stu1.emcA_Id==$scope.emcA_Id) {
                         EMCA_CategoryName = stu1.emcA_CategoryName;
                    }
                })
                angular.forEach($scope.class_list, function (stu2) {
                    if (stu2.asmcL_Id==$scope.ASMCL_Id) {
                         ASMCL_ClassName = stu2.asmcL_ClassName;
                    }
                })
                angular.forEach($scope.section_list, function (stu3) {
                    if (stu3.asmS_Id1==true) {
                        selected_sections.push(stu3);
                    }
                })

                if($scope.selected_temp.length==0)
                {
                    $scope.selected_temp.push({ ASMAY_Id:$scope.ASMAY_Id,ASMAY_Year:ASMAY_Year,EMCA_Id:$scope.emcA_Id,EMCA_CategoryName:EMCA_CategoryName,ASMCL_Id:$scope.ASMCL_Id,ASMCL_ClassName:ASMCL_ClassName,sections:selected_sections});
                }
                else if($scope.selected_temp.length>0)
                {
                    var already_cnt=0;
                    angular.forEach($scope.selected_temp,function(opq){
                    
                        if(opq.ASMAY_Id==$scope.ASMAY_Id && opq.EMCA_Id==$scope.emcA_Id && opq.ASMCL_Id==$scope.ASMCL_Id)
                        {
                            opq.sections=selected_sections;
                            already_cnt+=1;
                        }
                    })
                    if(already_cnt==0)
                    {
                         $scope.selected_temp.push({ ASMAY_Id:$scope.ASMAY_Id,ASMAY_Year:ASMAY_Year,EMCA_Id:$scope.emcA_Id,EMCA_CategoryName:EMCA_CategoryName,ASMCL_Id:$scope.ASMCL_Id,ASMCL_ClassName:ASMCL_ClassName,sections:selected_sections});
                    }
                }
                $scope.clear_temp();
            }
            else {
                $scope.submitted2 = true;
            }
        };
        $scope.clear_temp = function () {
            
           // $scope.asmaY_Id = "";
          //  $scope.hrmE_Id = "";
            // $scope.ivrmulF_Id = "";

            if ($scope.selected_temp.length == 0)
            {
                $scope.emcA_Id = "";
                $scope.gridApi2.grid.clearAllFilters();
            }
            $scope.ASMCL_Id = "";
            $scope.section_list = $scope.temp_section_list;
            angular.forEach($scope.section_list, function (stu1) {
                stu1.asmS_Id1 = false;

            })

          
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";


        }
        $scope.delete = function (row, index) {
            
            for (var x = 0; x < $scope.selected_temp.length; x++) {
                if (x == index) {
                    $scope.selected_temp.splice(x, 1);
                }
            }
            $scope.clear_temp();
        }
        $scope.edit = function (row, index) {
            $scope.clear_temp();
            
            $scope.ASMAY_Id = row.ASMAY_Id;
            $scope.emcA_Id = row.EMCA_Id;
            $scope.ASMCL_Id = row.ASMCL_Id;

            $scope.get_cate_class();
            
           
            // $scope.hrmE_Id = selectedclass[i].hrmE_Id;
         
         
        }

        $scope.viewrecordspopup = function (employee, SweetAlert) {
            
            //$scope.editEmployee = employee.eycG_Id;
            //var pageid = $scope.editEmployee;
            //var data = {
            //    "ASMAY_Id": employee.asmaY_Id,
            //    "EMCA_Id": employee.emcA_Id,
            //    "ASMCL_Id": employee.asmcL_Id,
            //}

            apiService.create("exammastercategory/getalldetailsviewrecords", employee).
                    then(function (promise) {
                        
                        $scope.Category_Name = promise.view_cls_sections[0].emcA_CategoryName;
                        $scope.Class_Name = promise.view_cls_sections[0].asmcL_ClassName;
                        $scope.viewrecordspopupdisplay = promise.view_cls_sections;

                    })

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };


    }

})();