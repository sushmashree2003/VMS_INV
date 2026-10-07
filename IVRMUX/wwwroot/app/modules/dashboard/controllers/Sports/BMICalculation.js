
//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .controller('BMICalculationController', BMICalculationController);

//    BMICalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

//    function BMICalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
//        $scope.searchValue = "";
//        $scope.currentPage = 1;
//        $scope.itemsPerPage = 10;
//        $scope.SPCCESTR_Id = 0;
//        $scope.sort = function (key) {
//            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
//            $scope.sortKey = key;
//        }
//        $scope.loadgrid = function () {
//            apiService.getURI("BMICalculation/loadgrid/", 1).then(function (promise) {
//                $scope.academicYear = promise.academicYear;
//                $scope.classList = promise.classList;
//                $scope.sectionList = promise.sectionList;
//                $scope.studentList = promise.studentList;
//                $scope.datareport = promise.datareport;

//                if (promise.count > 0) {
//                    $scope.eventsStudentRecordList = promise.eventsStudentRecordList;
//                    $scope.presentCountgrid = $scope.eventsStudentRecordList.length;
//                }
//                $scope.cancel();
//            });
//        }

//        $scope.get_section = function () {
//            $scope.studentList = "";
//            $scope.asmS_Id = "";
//            $scope.sectionList = "";
//            var data = {
//                "ASMAY_Id": $scope.ASMAY_Id,
//                "ASMCL_Id": $scope.asmcL_Id
//            }
//            apiService.create("BMICalculation/get_section", data)
//                .then(function (promise) {
//                    $scope.sectionList = promise.sectionList;
//                })
//        }

//        $scope.SPCCBMI_ID = 0;
//        $scope.getStudent = function (sectionId, asmclid) {
//            var data = {
//                "ASMCL_Id": asmclid,
//                "ASMS_Id": sectionId
//            }
//            apiService.create("BMICalculation/getStudents", data).then(function (promise) {
//                if (promise.studentList != null) {
//                    $scope.studentList = promise.studentList;
//                }

//            })
//        }

//        $scope.getBMI = function () {

//            var data = {
//                "AMST_Weight": $scope.AMST_Weight,
//                "AMST_Height": $scope.AMST_Height,
//            }
//            //var height_in_mts = $scope.AMST_Height * 0.3048;
//            //var height = height_in_mts * height_in_mts;
//            //$scope.AMST_BMI = $scope.AMST_Weight / height;
//            //var z = $scope.AMST_BMI;
//            //if ((z >= 11) && (z <= 18)) {
//            //    $scope.SPCC_BMI_Remarks = "Underweight";
//            //}
//            //else if ((z >= 18) && (z <= 21)) {
//            //    $scope.SPCC_BMI_Remarks = "Normal";
//            //}
//            //else if ((z >= 21) && (z <= 23)) {
//            //    $scope.SPCC_BMI_Remarks = "Overweight";
//            //}
//            //else if ((z >= 23) && (z <= 100)) {
//            //    $scope.SPCC_BMI_Remarks = "obese";
//            //}
//            //else if (z <= 11) {
//            //    $scope.SPCC_BMI_Remarks = "obese";
//            //}
//            //else {
//            //    $scope.SPCC_BMI_Remarks = "";
//            //}
//            var height_in_mts = $scope.AMST_Height / 100;
//            var metersqr = height_in_mts * height_in_mts;
//            $scope.AMST_BMI = $scope.AMST_Weight / metersqr;
//           var z = $scope.AMST_BMI;
//            if (z <= 18.5){
//                $scope.SPCC_BMI_Remarks = "Underweight";
//            }
//            else if ((z >= 18.5) && (z <= 24.9)) {
//                $scope.SPCC_BMI_Remarks = "Normal";
//            }
//            else if ((z >= 25) && (z <= 29.9)) {
//                $scope.SPCC_BMI_Remarks = "Overweight";
//            }            
//            else if ((z >= 30.0) && (z <= 39.5)){
//                $scope.SPCC_BMI_Remarks = "Obese";
//            }
//            else if (z > 39.5){
//                $scope.SPCC_BMI_Remarks = "Extreme Obese";
//            }

//        }


//        $scope.submitted = false;
//        $scope.saveRecord = function () {
//            if ($scope.myForm.$valid) {
//                debugger
//                $scope.albumNameArray1 = [];
//                angular.forEach($scope.studentList, function (role) {
//                    if (role.selected) $scope.albumNameArray1.push(role);
//                })

//                var bmicalculated = Math.round($scope.AMST_BMI * 100) / 100; 

//                var obj = {
//                    "SPCCBMI_ID": $scope.SPCCBMI_ID,
//                    "ASMAY_Id": $scope.ASMAY_Id,
//                    "ASMCL_Id": $scope.asmcL_Id,
//                    "ASMS_Id": $scope.asmS_Id,
//                    student: $scope.albumNameArray1,
//                    "AMST_Weight": $scope.AMST_Weight,
//                    "AMST_Height": $scope.AMST_Height,
//                    "AMST_BMI": bmicalculated,
//                    "SPCC_BMI_Remarks": $scope.SPCC_BMI_Remarks
//                }
//                apiService.create("BMICalculation/saveRecord", obj).
//                    then(function (promise) {
//                        if (promise.returnVal == 'saved') {
//                            swal("Record saved Successfully");
//                            $scope.loadgrid();
                           
//                        }
//                        if (promise.returnVal == 'duplicate') {
//                            swal("Two Records already exists for this student");
//                            $scope.loadgrid();

//                        }
//                        else if (promise.returnVal == "savingFailed") {
//                            swal("Failed to save record");
//                        }
//                        $scope.loadgrid();

//                    });
//            }
//            else {
//                $scope.submitted = true;
//            }
//        }

//        $scope.Print = function () {
//            var innerContents = '';
//            innerContents = document.getElementById("printSectionId").innerHTML;
//            var popupWinindow = window.open('');
//            popupWinindow.document.open();
//            popupWinindow.document.write('<html><head>' +
//                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
//                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
//                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
//                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
//            );
//            popupWinindow.document.close();
//        }

//        $scope.cancel = function () {

//            $scope.SPCCBMI_ID = 0;
//            $scope.ASMAY_Id = "";
//            $scope.asmcL_Id = "";
//            $scope.asmS_Id = "";
//            $scope.studentList = "";
//            $scope.AMST_Weight = "";
//            $scope.AMST_Height = "";
//            $scope.AMST_BMI = "";
//            $scope.SPCC_BMI_Remarks = "";
//            $scope.Cumureport = false;
//            $scope.screport = false;
//            $scope.submitted = false;
//            $scope.myForm.$setPristine();
//            $scope.myForm.$setUntouched();

//            angular.forEach($scope.studentList, function (option9) {
//                option9.selected = false;
//            });
//        }
//        $scope.interacted = function (field) {
//            return $scope.submitted;
//        };

//        $scope.isOptionsRequired3 = function () {

//            return !$scope.studentList.some(function (options) {
//                return options.selected;
//            });

//        }

//        $scope.addColumn4 = function () {
//            $scope.selected = $scope.studentList.every(function (itm) { return itm.selected; });
//        };

//    }
//})();



(function () {
    'use strict';

    angular
        .module('app')
        .controller('BMICalculationController', BMICalculationController);

    BMICalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];

    function BMICalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("BMICalculation/loadgrid/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                //$scope.studentList = promise.studentList;
                

                $scope.studentrecord = promise.studentrecord;

                if (promise.count > 0) {
                    $scope.eventsStudentRecordList = promise.eventsStudentRecordList;
                    $scope.presentCountgrid = $scope.eventsStudentRecordList.length;
                }
                //$scope.cancel();
            });
        }

        $scope.get_section = function () {
            debugger;
            $scope.studentList = "";
            $scope.asmS_Id = "";
            $scope.sectionList = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("BMICalculation/get_section", data)
                .then(function (promise) {
                    $scope.sectionList = promise.sectionList;
                })
        }

        
        $scope.getStudent = function () {
            debugger;
            $scope.showstdtable = false
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("BMICalculation/getStudents", data).then(function (promise) {
                if (promise.studentList != null) {
                    $scope.studentList = promise.studentList;
                }
                else {
                    swal('Students are not Available for this section');
                }
                

            })
        }
        //========================BMI Calculation
        $scope.getBMI = function (item) {
            debugger;

            var height_in_mts = item.spccshW_Height / 100;
            var metersqr = height_in_mts * height_in_mts;
            item.spccshW_BMI = item.spccshW_Weight / metersqr;

            var z = item.spccshW_BMI;
            if (z <= 18.5) {
                item.spccshW_BMIRemark = "Underweight";
            }
            else if ((z >= 18.5) && (z <= 24.9)) {
                item.spccshW_BMIRemark = "Normal";
            }
            else if ((z >= 25) && (z <= 29.9)) {
                item.spccshW_BMIRemark = "Overweight";
            }
            else if ((z >= 30.0) && (z <= 39.5)) {
                item.spccshW_BMIRemark = "Obese";
            }
            else if (z > 39.5) {
                item.spccshW_BMIRemark = "Extreme Obese";
            }

        }


        //===================================check box Selection
        $scope.check_allbox = function () {
            debugger;
            var toggleStatus1 = $scope.userselect;
            angular.forEach($scope.studentList, function (itm) {
                itm.selected = toggleStatus1;
            });
        }

        //================================================================================End
        $scope.userselect = "";
        $scope.get_studlistt = function () {

            debugger;
            $scope.userselect = $scope.studentList.every(function (options) {
                return options.selected;
            });
        }





        //=======================================Save
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                debugger
                $scope.albumNameArray1 = [];
                angular.forEach($scope.studentList, function (role) {
                    if (role.selected)
                        $scope.albumNameArray1.push(role);
                })
                if ($scope.albumNameArray1.length > 0) {
                    var asondate = $scope.spccshW_AsOnDate == null ? "" : $filter('date')($scope.spccshW_AsOnDate, "yyyy-MM-dd");
                    var obj = {
                        "SPCCSHW_Id": $scope.spccshW_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "SPCCSHW_AsOnDate": asondate,
                        student: $scope.albumNameArray1,
                    }
                    apiService.create("BMICalculation/saveRecord", obj).
                        then(function (promise) {
                            if (promise.returnVal == 'saved') {
                                swal("Record saved Successfully");
                                $state.reload();
                            }
                            if (promise.returnVal == 'duplicate') {
                                swal("Records already exists for this student");
                                $state.reload();
                            }
                            else if (promise.returnVal == "savingFailed") {
                                swal("Failed to save record");
                            }
                        });
                }
                else {
                    swal('Please Select Check Box');
                }
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.studentList = [];   
        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
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

        $scope.cancel = function () {

            $scope.SPCCBMI_ID = 0;
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.studentList =[];          
            $scope.Cumureport = false;
            $scope.screport = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.showstdtable = false;
            $scope.spccshW_AsOnDate = "";

            angular.forEach($scope.studentList, function (option9) {
                option9.selected = false;
            });
        }
        //===================================================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired3 = function () {

            return !$scope.studentList.some(function (options) {
                return options.selected;
            });

        }

        $scope.addColumn4 = function () {
            $scope.selected = $scope.studentList.every(function (itm) { return itm.selected; });
        };


        //================================
        $scope.showstdtable = false;
        $scope.get_student_info = function () {
            if ($scope.studentList.length > 0) {
                $scope.showstdtable = true;
            }
            else {
                $scope.showstdtable = false;
                swal('Students are not Available for this section');
            }
        }

        //==============================================Deactivate
       

        $scope.deactivate = function (user, SweetAlert) {
            debugger;
            $scope.lmaL_Id = user.lmaL_Id;

            var dystring = "";
            if (user.spccmhW_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.spccmhW_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("BMICalculation/deactivate", user).
                            then(function (promise) {
                                if (promise.returnVal2 == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        }


        //========================================================================edit record
        $scope.editdata = function (user) { 
            $scope.studentList = [];
            debugger;
            var data = {
                "SPCCSHW_Id": user.spccshW_Id,
                "ASMAY_Id": user.asmaY_Id,
                "ASMCL_Id": user.asmcL_Id,
                "ASMS_Id": user.asmS_Id,
            }
            apiService.create("BMICalculation/editdata", data)
                .then(function (promise) {
                    debugger;
                    $scope.editlist = promise.editlist;

                    $scope.spccshW_Id = promise.editlist[0].spccshW_Id;
                    $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                    $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                    $scope.asmS_Id = promise.editlist[0].asmS_Id;
                    $scope.amsT_Id = promise.editlist[0].amsT_Id;
                    $scope.spccshW_AsOnDate = new Date(promise.editlist[0].spccshW_AsOnDate);
                   

                    $scope.studdata = promise.studentList;
                    $scope.showstdtable = true;
                    console.log(promise.editlist);
                        angular.forEach($scope.studdata, function (tt) {
                            if (tt.amsT_Id == promise.editlist[0].amsT_Id) {
                                //tt.selected = true;
                                $scope.studentList.push({  spccshW_Height: promise.editlist[0].spccshW_Height, spccshW_Weight: promise.editlist[0].spccshW_Weight, spccshW_BMI: promise.editlist[0].spccshW_BMI, spccshW_BMIRemark: promise.editlist[0].spccshW_BMIRemark, amsT_Id: promise.editlist[0].amsT_Id, studentName: promise.editlist[0].studentName, amsT_AdmNo: promise.editlist[0].amsT_AdmNo });
                            }
                    });
                    console.log($scope.studentList);
                    angular.forEach($scope.studentList, function (ff) {
                        ff.selected = true;

                    }
                        
                        
                        )
                });
        }



    }
})();

