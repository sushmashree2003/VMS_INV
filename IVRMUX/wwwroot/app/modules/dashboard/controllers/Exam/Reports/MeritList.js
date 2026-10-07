(function () {
    'use strict';
    angular
.module('app')
.controller('MeritListController', MeritListController)

    MeritListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function MeritListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.studentAttendanceList = {};

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

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

        $scope.imgname = logopath;



        $scope.StuAttRptDropdownList = function () {
            //$scope.classname = false;
            //$scope.sectionname = false;

            apiService.get("MeritList/getdetails/").then(function (promise) {
                
                // $scope.all = true;
                $scope.yearDropdown = promise.yearlist;
                $scope.classDropdown = promise.classname;
                $scope.sectionDropdown = promise.secname;

                //for (var i = 0; i < $scope.yearDropdown.length; i++) {
                //    name = $scope.yearDropdown[i].asmaY_Id;
                //    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                //        if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                //            $scope.yearDropdown[i].Selected = true;
                //            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                //            //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                //        }
                //    }
                //}
                //$scope.imgname = promise.photopathname;
                ////$scope.classDropdown = promise.classlist;
                //$scope.sectionDropdown = promise.sectionList;
                //$scope.monthDropdown = promise.monthList;
            });
        }

        //$scope.toggleAll = function () {

        //    var toggleStatus = $scope.all2;
        //    angular.forEach($scope.students, function (itm) {
        //        itm.selected = toggleStatus;
        //        if ($scope.all2 == true) {
        //            $scope.printdatatable.push(itm);
        //        }
        //        else {
        //            $scope.printdatatable.splice(itm);
        //        }
        //    });
        //}


        $scope.toggleAll = function () {

            var toggleStatus = $scope.all2;
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amaY_RollNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.amsT_FirstName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_TotalMaxMarks)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_TotalObtMarks)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_Percentage)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_SectionRank)).indexOf($scope.searchValue) >= 0 ||
                  (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        }

        //$scope.optionToggled = function (SelectedStudentRecord, index) {
        //    
        //    $scope.all2 = $scope.students.every(function (itm)
        //    { return itm.selected; });
        //    if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
        //        $scope.printdatatable.push(SelectedStudentRecord);
        //    }
        //    else {
        //        $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
        //    }

        //}

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all2 = $scope.students.every(function (itm)
            { return itm.selected; });
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                if (itm.selected == true) {
                    $scope.printdatatable.push(itm);
                }
            });

        }

        $scope.printData = function () {

            //apiService.get("AttendanceReport/getdetails/").then(function (promise) {
            //    
            //    if (promise.photopathname != null) {
            //        $scope.imgname = promise.photopathname;
            //    }
            //});


            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
           '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
          );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (tableId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.submitted = false;

        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];


        $scope.savetmpldata = function () {

            $scope.printdatatable = [];
            $scope.searchValue = "";
           

            var data = {               

                "ASMAY_Id": $scope.asmaY_Id,
                 "ASMCL_Id": $scope.asmcL_Id,
                 "ASMS_Id": $scope.asmS_Id,
            }
            

            if ($scope.myForm.$valid) {
                    if ($scope.asmaY_Id == undefined || $scope.asmaY_Id == null) {

                        swal("Select The Fields !");
                    }
                    else {
                        apiService.create("MeritList/getAttendetails", data)
                   .then(function (promise) {
                       
                       $scope.date = new Date();
                       if (promise.studentAttendanceList.length > 0 && promise.studentAttendanceList != null) {
                           $scope.students = promise.studentAttendanceList;
                           $scope.presentCountgrid = $scope.students.length;

                         

                           $scope.gridflag = true;
                           $scope.print_flag = false;
                           $scope.excel_flag = false;
                       }
                       else {

                           swal("No record Found !");
                           $scope.gridflag = false;
                           $scope.print_flag = true;
                           $scope.excel_flag = true;
                       }
                   })
                    }
                }

          



            else {

                $scope.submitted = true;
            }
        }

        $scope.sortBy = function(propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

      

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {

            $state.reload();
            //if ($scope.type == 1)
            //{
            //    $scope.asmaY_Id = "";
            //    $scope.gridflag = false;


            //    $scope.excel_flag = true;
            //    $scope.print_flag = true;

            //    $scope.submitted = false;
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();

            //}

            //if ($scope.type == 2) {
            //    $scope.asmaY_Id = "";
            //    $scope.asmcL_Id = "";
            //    $scope.asmC_Id = "";
            //    $scope.gridflag = false;


            //    $scope.excel_flag = true;
            //    $scope.print_flag = true;

            //    $scope.submitted = false;
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();

            //}



        };

    }
})();
