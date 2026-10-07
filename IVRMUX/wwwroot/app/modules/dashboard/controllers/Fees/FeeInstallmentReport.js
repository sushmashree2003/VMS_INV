(function () {
    'use strict';
    angular
.module('app')
.controller('FeeInstallmentReportController', FeeInstallmentReportController)
    FeeInstallmentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function FeeInstallmentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout,$filter) {
        $scope.colarrayall = [];
      
        $scope.printdatatable = [];
        $scope.albumNameArraygroupids = [];
        $scope.repcanbtn = false;
        $scope.arrlist6 = [];
        $scope.classcount = [];
        $scope.sectioncount = [];
        $scope.groupcount = [];
        $scope.searchValue = "";

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        $scope.isOptionsRequired = function () {
            if ($scope.catg === 'Category') {
                return !$scope.arrlstinst.some(function (options1) {
                    return options1.selected;
                });
            }
            
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

       

        $scope.sort = function (keyname) {
            
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.sortBy1 = function (keyname) {
            
           
            if (keyname == "Netamount" || keyname == "Balance" || keyname == "Concession" || keyname == "Fine" || keyname == "Waivedoff") {
                keyname = keyname.toLowerCase();
            }
            if (keyname == "Paid") {
                keyname = "paidamount";
            }
            
            
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.groupwisehide = false;
        $scope.showbutton = true;
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.data, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
                if ($scope.printdatatable.length > 0) {
                    $scope.showbutton = true;
                }
                else {
                    $scope.showbutton = false;
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.data.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        

        $scope.exportToExcel = function (tableId) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
        
        }

        $scope.printData = function () {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                 '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.page1 = "page1";
            $scope.result = false;
            $scope.catg = "Category";
            $scope.onclickloaddata();
            var pageid = 1;
            apiService.getURI("FeeInstallmentReport/getdetails", pageid).
     then(function (promise) {

         $scope.arrlist6 = promise.adcyear;
         $scope.arrlistchk = promise.fillcategory;
         $scope.arrlstinst = promise.fillinstallment;
         $scope.classcount = promise.fillclass;
         $scope.routecount = promise.groupalldata;
         $scope.groupcount = promise.fillmastergroup;
     })
        }

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }


        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }

        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeInstallmentReport/getinstallmentid", data).
               then(function (promise) {
                   $scope.arrlstinst = promise.fillinstallment;
                   $scope.arrlstinst1 = promise.fillinstallment;
                   
                   
               })
        }



        $scope.onselectclass = function () {

            $scope.result = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeInstallmentReport/classes", data).
               then(function (promise) {
                   $scope.arrlstinst1 = promise.fillinstallment;
                   $scope.sectioncount = promise.fillsection;
               })
        }





        $scope.onselectgroup=function()
        {

            $scope.result = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "FMG_Id":$scope.fmG_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeInstallmentReport/group", data).
               then(function (promise) {
                 //  $scope.arrlstinst1 = promise.fillinstallment;
                   $scope.fillhead = promise.fillmasterhead;


                   angular.forEach($scope.fillhead, function (tr) {
                       tr.selected1 = true;
                   })
               })
        }
      



        $scope.frmdt = true;
        $scope.todt = true;

       
        $scope.onclickloaddata = function () {
            
            $scope.colarrayall = [];
            $scope.submitted = false;
            if ($scope.catg === 'Category') {
                $scope.categorywise = true;
                $scope.clswise = false;
                $scope.result = false;
                $scope.trpwise = false;
                $scope.year = true;
                $scope.repcanbtn = true;
              
            }
            else if ($scope.catg === 'Class') {
                $scope.categorywise = false;
                $scope.clswise = true;
                $scope.result = false;
                $scope.trpwise = false;
                $scope.year = true;
                $scope.repcanbtn = true;
              
            }
            else {
                $scope.categorywise = false;
                $scope.clswise = false;
                $scope.result = false;
                $scope.trpwise = true;
                $scope.year = true;
                $scope.repcanbtn = true;

            }
            
        };

        $scope.binddata = function (arrlstinst) {
            // swal(ftI_Id);
            
           
            angular.forEach($scope.arrlstinst, function (role) {
                if (!!role.selected) $scope.albumNameArraygroupids.push(role);
            })

           
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "temparrayinst": $scope.albumNameArraygroupids,
            }
          
            
            apiService.create("FeeInstallmentReport/getinstallmentid1", data).
               then(function (promise) {
                   $scope.arrlstinst1 = promise.fillinstallment;
                 
               })
        }


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.data, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.data.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.submitted = false;

        


        $scope.ShowReport = function () {
            
            $("#grid123").kendoGrid();
            $('#grid123').data('kendoGrid').dataSource.read();
            $('#grid123').data('kendoGrid').refresh();
            
            if ($scope.myForm.$valid) {
                var FMH_Ids = [];
            if ($scope.catg == 'Category') {
                $scope.fmcC_Id = $scope.fmcC_Id;
                $scope.asmcL_Id = 0;
                $scope.amsC_Id = 0;
                $scope.trmR_Id = 0;
                $scope.fmG_Id = 0;
                angular.forEach($scope.arrlstinst, function (role) {
                    if (!!role.selected) $scope.albumNameArraygroupids.push({ columnID: role.ftI_Id, columnName: role.ftI_Name });

                    $scope.header_list = [];
                    angular.forEach($scope.arrlstinst, function (role) {
                        if (role.selected) $scope.header_list.push(role);
                    })
                })

                angular.forEach($scope.arrlistchk, function (rt) {
                    if (rt.fmcC_Id == $scope.fmcC_Id) {
                        $scope.routename = rt.fmcC_ClassCategoryName;
                    }
                });

            }
            else if ($scope.catg == 'Class') {
                $scope.asmcL_Id = $scope.asmcL_Id;
                $scope.amsC_Id = $scope.amsC_Id;
                $scope.fmcC_Id = 0;
                $scope.trmR_Id = 0;
                $scope.fmG_Id = 0;
                $scope.header_list = [];
                angular.forEach($scope.arrlstinst1, function (role) {
                    $scope.header_list.push(role);
                })
                angular.forEach($scope.classcount, function (rt) {
                    if (rt.asmcL_Id == $scope.asmcL_Id) {
                        $scope.routename = rt.asmcL_ClassName;
                    }
                });

                }
            else {
                $scope.asmcL_Id = 0;
                $scope.amsC_Id = 0;
                $scope.fmcC_Id = 0;
                $scope.trmR_Id = $scope.trmR_Id;
                $scope.fmG_Id = $scope.fmG_Id;
                $scope.header_list = [];
                angular.forEach($scope.arrlstinst1, function (role) {
                    $scope.header_list.push(role);
                })
                angular.forEach($scope.routecount, function (rt) {
                    if (rt.trmR_Id == $scope.trmR_Id) {
                        $scope.routename = rt.trmR_RouteNo + ' : ' + rt.trmR_RouteName;
                    }
                });
                angular.forEach($scope.fillhead, function (ty) {
                    if (ty.selected1) {
                        FMH_Ids.push(ty.fmH_Id);
                    }
                })
            }
           


                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "FTI_Id": $scope.ftI_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "AMSC_Id": $scope.amsC_Id,
                    "FMCC_Id": $scope.fmcC_Id,
                    "reporttype": $scope.catg,
                    "FMG_Id": $scope.fmG_Id,
                    "TRMR_Id": $scope.trmR_Id,
                    "temparrayinst1": $scope.albumNameArraygroupids,
                    FMGG_Ids: FMH_Ids,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeInstallmentReport/radiobtndata", data).
              then(function (promise) {
                  $('#grid123').empty();
                  

                  $scope.totcountfirstnew = promise.classwisedata.length;
                  $scope.result = true;
                  var installmentcount = $scope.arrlstinst1.length;
                 

                  $scope.insarray = [{ name: "Netamount" }, { name: "Paid" }, { name: "Balance" }, { name: "Concession" }, { name: "Fine" }, { name: "Waivedoff" }];
                  $scope.columns1 = [{  field: "Netamount", name: "Netamount", title: "Netamount"}, {field: "Paid", name: "Paid", title: "Paid" }, {field: "Balance", name: "Balance", title: "Balance"}, {field: "Concession", name: "Concession", title: "Concession"}, {field: "Fine", name: "Fine", title: "Fine"}, {field: "Waivedoff", name: "Waivedoff", title: "Waivedoff"}];
                  $scope.newarray = [];
                  var finalarray = 0;
                  finalarray = Number(installmentcount)
                  for (var i = 0; i < $scope.header_list.length; i++) {
                      for (var j = 0; j < 6; j++) {
                          $scope.newarray.push({ name: $scope.insarray[j].name, id: $scope.header_list[i].ftI_Id, name1: "hema" + $scope.header_list[i].ftI_Id + $scope.insarray[j].name });

                      }

                  }
                  $scope.newarray1 = $scope.newarray;


                  // start
                  $scope.data = promise.classwisedata;

                  //var obj = promise.classwisedata[0];
                  //$scope.students = Object.keys(obj).filter(function (key) {
                  //    if (obj.hasOwnProperty(key) && typeof key === 'string') {
                  //        return key;
                  //    }
                  //});

                  //$scope.format = function (str) {
                  //    return str.replace('_', ' '); //do the rest of the formatting here. 
                  //}

                  //end

                  //MB
                  if(promise.classwisedata!=null && promise.classwisedata!="" && promise.classwisedata.length>0)
                  {
                      
                      var stu_list_new = [];
                      angular.forEach(promise.classwisedata, function (op1) {
                          var stu_id = op1.AMST_Id;
                          var list_stu = [];
                          angular.forEach(promise.classwisedata, function (op2) {
                              if(op2.AMST_Id==stu_id)
                              {
                                  var coun = 0;
                                  angular.forEach($scope.header_list, function (op) {
                                      if (op2.FTI_Id == op.ftI_Id) {
                                          list_stu.push({ FTI_Id: op2.FTI_Id, Netamount: op2.Netamount, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Fine: op2.Fine, Waivedoff: op2.Waivedoff })
                                          coun += 1;
                                      }

                                  })
                                  if (coun == 0)
                                  {
                                      list_stu.push({ FTI_Id: op2.FTI_Id, Netamount: op2.Netamount, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Fine: op2.Fine, Waivedoff: op2.Waivedoff });
                                  }
                                
                              }


                          })
                          if (stu_list_new.length == 0)
                          {
                              stu_list_new.push({ AMST_Id: stu_id, AMST_FirstName: op1.AMST_FirstName, ASMCL_ClassName: op1.ASMCL_ClassName, ASMC_SectionName: op1.ASMC_SectionName, AMAY_RollNo: op1.AMAY_RollNo  ,Installment_Reports: list_stu });
                          }
                          else if(stu_list_new.length>0)
                          {
                              var already_cnt = 0;
                              angular.forEach(stu_list_new, function (td) {
                                  if(td.AMST_Id==stu_id)
                                  {
                                      already_cnt += 1;
                                  }
                              })
                              if(already_cnt==0)
                              {
                                  stu_list_new.push({ AMST_Id: stu_id, AMST_FirstName: op1.AMST_FirstName, ASMCL_ClassName: op1.ASMCL_ClassName, ASMC_SectionName: op1.ASMC_SectionName, AMAY_RollNo: op1.AMAY_RollNo, Installment_Reports: list_stu });
                              }
                          }
                         
                      })
                      
                    
                      angular.forEach(stu_list_new, function (obj) {
                          angular.forEach(obj.Installment_Reports, function (obj1) {
                              angular.forEach($scope.newarray1, function (x) {
                                  if(x.id==obj1.FTI_Id)
                                  {
                                      obj[x.name1] = obj1[x.name];
                                    
                                  }
                              })

                          })
                      })

                      $scope.totcountfirstnew = stu_list_new.length;
                      $scope.data = stu_list_new;


                      //hema

                     
                 
                   //   $scope.mainsubhdr = [];
                      angular.forEach($scope.header_list, function (op) {
                          op.columns = [];
                          angular.forEach($scope.columns1, function (op123) {
                          
                              op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title ,width:100});

                          })
                         op.title = op.ftI_Name;
                      })
                   
                      $scope.colarrayall = [{

                          title: "SLNO",
                          template: "<span class='row-number'></span>", width: 75

                      },
         {
             name: 'AMAY_RollNo', field: 'AMAY_RollNo', title: 'Roll No', width: 200
             },
     {
         name: 'AMST_FirstName', field: 'AMST_FirstName', title: 'Student Name', width: 200
     },
        {
            name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: 100
        },
        {
            name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: 100
        }
                      ];


                      angular.forEach($scope.header_list, function (obj) {
                          $scope.colarrayall.push(obj);
                      })
                      console.log($scope.header_list);
                     
                      console.log($scope.colarrayall);

                      $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;

                      console.log($scope.txtdata);
                      $scope.aaaa = [{
                          title: $scope.txtdata,
                          columns: $scope.colarrayall
                      }]


                      console.log($scope.data);
                      var gridall;

                      $(document).ready(function () {
                          initGridall();
                          $(".k-grid-toolbar").prepend('<div class="gridTitle"><h4  style="color:white;" class="titlecolor">' + "Report For" + "   " + $scope.routename + '</h4></div>');
                      });

                      //$scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;
                      //console.log($scope.txtdata);
                      //$scope.aaaa = [{
                      //    title: $scope.txtdata,
                      //    columns: $scope.colarrayall
                      //}]


                      function initGridall() {
                          gridall = $("#grid123").kendoGrid({
                              toolbar: ["excel", "pdf"],
                              excel: {
                                  fileName: "Kendo UI Grid Export.xlsx",
                                  proxyURL: "",
                                  filterable: true
                              },
                              pdf: {
                                  fileName: "Kendo UI Grid Export.pdf"
                              },
                              dataSource: {
                                  //type: "odata",
                                  //transport: {
                                  //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                  //},
                                  data: $scope.data
                              },
                              sortable: true,
                              pageable: false,
                              groupable: false,
                              filterable: true,
                              columnMenu: true,
                              reorderable: true,
                              resizable: true,
                              columns: $scope.aaaa,
                              dataBound: function () {
                                  var rows = this.items();
                                  $(rows).each(function () {
                                      var index = $(this).index() + 1;
                                      var rowLabel = $(this).find(".row-number");
                                      $(rowLabel).html(index);
                                  });
                              }
                          }).data("kendoGrid");
                          gridall.setOptions({
                              sortable: true
                          });
                      }
                      
                      //hema


                  }
                  //MB
                  
                    })
                
            }

            else {
                $scope.submitted = true;
            }          
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();
