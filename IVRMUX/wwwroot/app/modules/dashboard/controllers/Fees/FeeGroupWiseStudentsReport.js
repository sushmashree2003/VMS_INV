(function () {
    'use strict';
    angular
.module('app')
.controller('FeeGroupWiseStudentReportController', FeeGroupWiseStudentReportController)
    FeeGroupWiseStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function FeeGroupWiseStudentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.searchString = "";
        $scope.print = false;



        $scope.loadbasicdata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeGroupWiseStudentReport/getinitialfeedata/",pageid).then(function (promise) {
                
                $scope.yearlist = promise.yearList;
               // $scope.fee_group_list = promise.fee_Group_List;
              //  $scope.class_list = promise.class_List;
                $scope.section_list = promise.section_List;
                $scope.student_name_list = promise.student_Name_List;
                $scope.class_category_list = promise.class_Category_List;
                $scope.radio_val = "all";
             //   $scope.getVal();
            });
        }
        $scope.getVal = function () {
            
            $scope.asmaY_Id = "";
            $scope.fmG_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.stuD_Id = "";
            $scope.fmcC_Id = "";
            $scope.showgrid = false;

            if ($scope.radio_val == "student_wise" || $scope.radio_val == "admCat") {
                $scope.amntchecked = true;
            }
            else {
                $scope.amntchecked = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.printdatatable = [];
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sortBy = function (keyname) {
            
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        

     
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.printdatatable = [];
            $scope.all = false;
            if ($scope.myForm.$valid) {
                
                var selectedVal = $scope.radio_val;
                if (selectedVal == "all") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.fmG_Id != "" && $scope.fmG_Id != undefined) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "FMG_Id": $scope.fmG_Id,

                    }
                }
                else if (selectedVal == "individual") {

                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        && $scope.fmG_Id != "" && $scope.fmG_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                        ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "FMG_Id": $scope.fmG_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "Stud_Sel": $scope.stud_sel,
                    }
                }
                else if (selectedVal == "student_wise") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                          && $scope.stuD_Id != "" && $scope.stuD_Id != undefined
                        ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "Stud_Id": $scope.stuD_Id,

                    }
                }
                else if (selectedVal == "admCat") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                        && $scope.fmcC_Id != "" && $scope.fmcC_Id != undefined
                        ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "FMCC_Id": $scope.fmcC_Id, //category    

                    }
                }

                if ($scope.validate) {
                    apiService.create("FeeGroupWiseStudentReport/", data).then(function (promise) {
                        
                        $scope.validate = false;
                        if (promise.fhwR_searchdatalist.length > 0) {
                            $scope.searchdatalist = promise.fhwR_searchdatalist;
                            $scope.totcountfirst = promise.fhwR_searchdatalist.length;
                            $scope.showgrid = true;
                            $scope.classFlag = false;
                            $scope.sectionFlag = false;
                            $scope.rollNo = false;
                            $scope.feeGrp = false;
                            $scope.amtFlag = false;
                            $scope.print = true;
                            if ($scope.checked == true) {
                                $scope.feeGrp = true;
                            }
                            if ($scope.amntchecked == true) {
                                $scope.checked = true; $scope.feeGrp = true; $scope.amtFlag = true;
                            }
                        }
                        else {
                            $scope.showgrid = false;
                            $scope.print = false;
                            swal("No Records Found !!!");
                        }
                    })
                }
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.onselectyrdrpdwn = function (asmaY_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {
            $scope.yearid = asmaY_Id;
                var data = {
                    "ASMAY_Id": asmaY_Id
                }
                
                apiService.create("FeeGroupWiseStudentReport/Getclass/", data).then(function (promise) {
                    
                 
                    if (promise.class_List.length > 0) {
                        $scope.class_list = promise.class_List;
                        $scope.fee_group_list = promise.fee_Group_List;

                    }
                    //else {
                    //    swal("No Class Found");
                    //}
                    if (promise.fee_Group_List.length>0) {
                        $scope.fee_group_list = promise.fee_Group_List;
                        $scope.class_list = promise.class_List;
                    }
                    //else {
                    //    swal("No Group Found");
                    //}
                    //$scope.yearlist = promise.yearList;
                    //$scope.fee_group_list = promise.fee_Group_List;
                    //$scope.class_list = promise.class_List;
                    //$scope.section_list = promise.section_List;
                    //$scope.student_name_list = promise.student_Name_List;
                    //$scope.class_category_list = promise.class_Category_List;
                    //$scope.radio_val = "all";
                    //$scope.getVal();
                });
           // }
      

        }




        

        $scope.onselectclsdrpdwn = function (asmcL_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {
            $scope.class=asmcL_Id;
            var data = {
                "ASMAY_Id": $scope.yearid,
                "ASMCL_Id": asmcL_Id
            }
            
            apiService.create("FeeGroupWiseStudentReport/GetSection/", data).then(function (promise) {
                
                //$scope.section_list = promise.Section_List;
                if (promise.section_List.length > 0) {
                    $scope.section_list = promise.section_List;

                }
               
                //$scope.yearlist = promise.yearList;
                //$scope.fee_group_list = promise.fee_Group_List;
                //$scope.class_list = promise.class_List;
                //$scope.section_list = promise.section_List;
                //$scope.student_name_list = promise.student_Name_List;
                //$scope.class_category_list = promise.class_Category_List;
                //$scope.radio_val = "all";
                //$scope.getVal();
            });
            // }


        }
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.exportToExcel = function (tableId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

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

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue, function (itm) {
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
            
            $scope.all = $scope.searchdatalist.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.onselectsectiondrpdwn = function (asmS_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {

            var data = {
                "ASMAY_Id": $scope.yearid,
                "ASMS_Id": asmS_Id,
                "ASMCL_Id": $scope.class
            }
            
            apiService.create("FeeGroupWiseStudentReport/GetStudent/", data).then(function (promise) {
                

                if (promise.student_Name_List.length > 0) {
                    $scope.student_name_list = promise.student_Name_List;

                }
                //else {
                //    swal("No Record Found");
                //}
                //$scope.yearlist = promise.yearList;
                //$scope.fee_group_list = promise.fee_Group_List;
                //$scope.class_list = promise.class_List;
                //$scope.section_list = promise.section_List;
                //$scope.student_name_list = promise.student_Name_List;
                //$scope.class_category_list = promise.class_Category_List;
                //$scope.radio_val = "all";
                //$scope.getVal();
            });
            // }


        }
    }
})();







